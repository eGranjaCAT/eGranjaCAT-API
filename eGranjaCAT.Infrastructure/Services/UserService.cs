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
            var resultObj = new ServiceResult<AuthResponseDTO>();

            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Error creant usuari: {error.Description}");
                        resultObj.Errors.Add(error.Description);
                    }
                    resultObj.Success = false;
                    return resultObj;
                }

                var roleName = userDTO.Role.ToString();
                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError($"Error creant el rol: {error.Description}");
                            resultObj.Errors.Add(error.Description);
                        }
                        resultObj.Success = false;
                        return resultObj;
                    }
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!addToRoleResult.Succeeded)
                {
                    foreach (var error in addToRoleResult.Errors)
                    {
                        _logger.LogError($"Error asignant el rol: {error.Description}");
                        resultObj.Errors.Add(error.Description);
                    }
                    resultObj.Success = false;
                    return resultObj;
                }

                foreach (var perm in userDTO.Permissions)
                {
                    var claim = new Claim("Access", perm);
                    var claimResult = await _userManager.AddClaimAsync(user, claim);
                    if (!claimResult.Succeeded)
                    {
                        foreach (var error in claimResult.Errors)
                            _logger.LogError($"Error adding claim {perm}: {error.Description}");
                    }
                }


                await _context.SaveChangesAsync();

                var variables = new Dictionary<string, string>
                {
                    { "Name", userDTO.Name },
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
                var token = await BuildJwtToken(tokenDTO);

                resultObj.Data = token;
                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al crear el usuario.");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperado al crear el usuario.");
                return resultObj;
            }
        }



        public async Task<ServiceResult<AuthResponseDTO>> LoginUserAsync(LoginUserDTO loginDTO)
        {
            var resultObj = new ServiceResult<AuthResponseDTO>();

            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    _logger.LogWarning("Inici de sessió fallit: usuari no trobat amb el correu {Email}", loginDTO.Email);
                    resultObj.Errors.Add("Correu o contrasenya invàlids.");
                    resultObj.Success = false;
                    return resultObj;
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (!passwordCheck)
                {
                    _logger.LogWarning("Inici de sessió fallit: contrasenya incorrecta per a l'usuari {Email}", loginDTO.Email);
                    resultObj.Errors.Add("Correu o contrasenya invàlids.");
                    resultObj.Success = false;
                    return resultObj;
                }

                var tokenDTO = _mapper.Map<TokenUserDTO>(user);

                var roles = await _userManager.GetRolesAsync(user);
                tokenDTO.Role = roles.Any() && Enum.TryParse<UserRoles>(roles.First(), out var role) ? role.ToString() : UserRoles.User.ToString();

                var token = await BuildJwtToken(tokenDTO);
                resultObj.Data = token;
                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en iniciar sessió per a l'usuari {Email}", loginDTO.Email);
                resultObj.Errors.Add("S'ha produït un error inesperat en iniciar sessió. Torna-ho a intentar més tard.");
                resultObj.Success = false;
                return resultObj;
            }
        }



        public async Task<ServiceResult<bool>> DeleteUserById(Guid id)
        {
            var resultObj = new ServiceResult<bool>();

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    _logger.LogWarning("Usuari amb ID {Id} no trobat.", id);
                    resultObj.Errors.Add($"L'usuari amb ID {id} no s'ha trobat.");
                    resultObj.Success = false;
                    return resultObj;
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    foreach (var error in deleteResult.Errors)
                    {
                        _logger.LogError("Error en eliminar l'usuari amb ID {Id}: {Error}", id, error.Description);
                        resultObj.Errors.Add(error.Description);
                    }
                    resultObj.Success = false;
                    return resultObj;
                }

                resultObj.Data = true;
                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepció inesperada en intentar eliminar l'usuari amb ID {Id}", id);
                resultObj.Errors.Add("S'ha produït un error inesperat en eliminar l'usuari.");
                resultObj.Success = false;
                return resultObj;
            }
        }


        public async Task<ServiceResult<List<GetUserDTO>>> GetUsersAsync()
        {
            var resultObj = new ServiceResult<List<GetUserDTO>>();

            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDTOs = _mapper.Map<List<GetUserDTO>>(users);

                if (userDTOs == null || !userDTOs.Any())
                {
                    _logger.LogInformation("No s'han trobat usuaris.");
                    resultObj.Data = new List<GetUserDTO>();
                    resultObj.Success = true;
                    return resultObj;
                }

                resultObj.Data = userDTOs;
                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en obtenir la llista d'usuaris.");
                resultObj.Errors.Add("S'ha produït un error inesperat en obtenir els usuaris.");
                resultObj.Success = false;
                return resultObj;
            }
        }

        public async Task<ServiceResult<GetUserDTO?>> GetUserByIdAsync(Guid id)
        {
            var resultObj = new ServiceResult<GetUserDTO?>();

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var userDTO = _mapper.Map<GetUserDTO>(user);

                if (userDTO == null)
                {
                    _logger.LogWarning("Usuari amb ID {Id} no trobat.", id);
                    resultObj.Errors.Add($"L'usuari amb ID {id} no s'ha trobat.");
                    resultObj.Success = false;
                    return resultObj;
                }

                resultObj.Data = userDTO;
                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat en obtenir l'usuari amb ID {Id}", id);
                resultObj.Errors.Add("S'ha produït un error inesperat en obtenir l'usuari.");
                resultObj.Success = false;
                return resultObj;
            }
        }



        private async Task<AuthResponseDTO> BuildJwtToken(TokenUserDTO userDTO)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id),
                new Claim(ClaimTypes.Name, userDTO.Email),
                new Claim(ClaimTypes.Role, userDTO.Role.ToString())
            };

            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            var userClaims = await _userManager.GetClaimsAsync(user!);
            claims.AddRange(userClaims);

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