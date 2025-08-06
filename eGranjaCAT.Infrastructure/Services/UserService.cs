using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Auth;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Enums;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace eGranjaCAT.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger, IConfiguration configuration, IMapper mapper, IEmailService emailService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _emailService = emailService;
            _context = context;
        }

        public async Task<ServiceResult<AuthResponseDTO>> CreateUserAsync(CreateUserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded) return ServiceResult<AuthResponseDTO>.Fail("Error creant l'usuari: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                var roleName = userDTO.Role.ToString();
                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded) return ServiceResult<AuthResponseDTO>.Fail("Error creant el rol: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!addToRoleResult.Succeeded) return ServiceResult<AuthResponseDTO>.Fail("Error afegint l'usuari al rol: " + string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));

                foreach (var perm in userDTO.Permissions)
                {
                    var claim = new Claim("Access", perm.ToString());
                    var claimResult = await _userManager.AddClaimAsync(user, claim);
                    if (!claimResult.Succeeded) return ServiceResult<AuthResponseDTO>.Fail("Error afegint permís a l'usuari: " + string.Join(", ", claimResult.Errors.Select(e => e.Description)));
                }

                await _context.SaveChangesAsync();

                var variables = new Dictionary<string, string>
                {
                    { "Name", $"{userDTO.Name} {userDTO.Lastname}" },
                    { "Email", userDTO.Email },
                    { "Role", userDTO.Role.ToString() },
                    { "RegistrationDate", DateTime.Now.ToString("dd/MM/yyyy") }
                };

                var emailResult = await _emailService.SendEmailAsync(userDTO.Email, "Benvingut a la nostra plataforma!", "Benvinguda.html", variables);
                if (!emailResult.Success)
                {
                    _logger.LogError("Error enviando correo de bienvenida: " + string.Join(", ", emailResult.Errors));
                }

                var tokenDTO = _mapper.Map<TokenUserDTO>(user);
                tokenDTO.Role = roleName;

                var token = await BuildJwtToken(tokenDTO);

                return ServiceResult<AuthResponseDTO>.Ok(token, 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al crear el usuario.");

                return ServiceResult<AuthResponseDTO>.FromException(ex);
            }
        }



        public async Task<ServiceResult<AuthResponseDTO>> LoginUserAsync(LoginUserDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null) return ServiceResult<AuthResponseDTO>.Fail("Usuari no trobat.");

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (!passwordCheck) return ServiceResult<AuthResponseDTO>.Fail("Contrasenya incorrecta.");

                var tokenDTO = _mapper.Map<TokenUserDTO>(user);

                var roles = await _userManager.GetRolesAsync(user);
                tokenDTO.Role = roles.Any() && Enum.TryParse<RolesEnum>(roles.First(), out var role) ? role.ToString() : RolesEnum.User.ToString();

                var token = await BuildJwtToken(tokenDTO);

                return ServiceResult<AuthResponseDTO>.Ok(token, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en iniciar sessió per a l'usuari {Email}", loginDTO.Email);

                return ServiceResult<AuthResponseDTO>.FromException(ex);
            }
        }


        public async Task<ServiceResult<bool>> DeleteUserById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null) return ServiceResult<bool>.Fail($"Usuari amb ID {id} no trobat.");

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded) return ServiceResult<bool>.Fail("Error eliminant l'usuari: " + string.Join(", ", deleteResult.Errors.Select(e => e.Description)));

                return ServiceResult<bool>.Ok(true, 204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepció inesperada en intentar eliminar l'usuari amb ID {Id}", id);

                return ServiceResult<bool>.FromException(ex);
            }
        }


        public async Task<ServiceResult<List<GetUserDTO>>> GetUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDTOs = _mapper.Map<List<GetUserDTO>>(users);

                return ServiceResult<List<GetUserDTO>>.Ok(userDTOs, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en obtenir la llista d'usuaris.");

                return ServiceResult<List<GetUserDTO>>.FromException(ex);
            }
        }

        public async Task<ServiceResult<GetUserDTO?>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var userDTO = _mapper.Map<GetUserDTO>(user);

                if (userDTO == null) return ServiceResult<GetUserDTO?>.Fail($"Usuari amb ID {id} no trobat.");

                return ServiceResult<GetUserDTO?>.Ok(userDTO, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en obtenir l'usuari amb ID {Id}", id);

                return ServiceResult<GetUserDTO?>.FromException(ex);
            }
        }



        private async Task<AuthResponseDTO> BuildJwtToken(TokenUserDTO userDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id ?? throw new ArgumentNullException(nameof(userDTO.Id))),
                new Claim(ClaimTypes.Email, userDTO.Email ?? throw new ArgumentNullException(nameof(userDTO.Email))),
                new Claim(ClaimTypes.Role, userDTO.Role.ToString() ?? throw new ArgumentNullException(nameof(userDTO.Role)))
            };

            string fullName;
            if (!string.IsNullOrWhiteSpace(userDTO.Name) || !string.IsNullOrWhiteSpace(userDTO.Lastname))
            {
                var parts = new[] { userDTO.Name, userDTO.Lastname }
                                .Where(s => !string.IsNullOrWhiteSpace(s));
                fullName = string.Join(" ", parts);
            }
            else
            {
                fullName = userDTO.Email!;
            }

            claims.Add(new Claim(ClaimTypes.Name, fullName));

            var userEntity = await _userManager.FindByEmailAsync(userDTO.Email);
            if (userEntity != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(userEntity);
                claims.AddRange(userClaims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(1);

            var secToken = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(secToken);

            return new AuthResponseDTO { Token = token, Expiraton = expires };
        }
    }
}