using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace eGranjaCAT.Shared.Utils
{
    public class ValidationsUtils
    {
        public static ValidationResult ValidateNif(string nif, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(nif)) return new ValidationResult("El NIF és obligatori.");

            var regex = new Regex(@"^\d{8}[A-Z]$");
            if (!regex.IsMatch(nif)) return new ValidationResult("El NIF ha de tenir 8 dígits seguits d'una lletra majúscula.");

            string letras = "TRWAGMYFPDXBNJZSQVHLCKE";
            int num = int.Parse(nif.Substring(0, 8));
            char letraCorrecta = letras[num % 23];
            if (nif[8] != letraCorrecta)
                return new ValidationResult("La lletra del NIF no és correcta.");

            return ValidationResult.Success!;
        }

        public static ValidationResult ValidateDate(object value, ValidationContext context)
        {
            string dataSortidaString = value as string;
            if (string.IsNullOrWhiteSpace(dataSortidaString)) return new ValidationResult("El valor es requerido.");

            bool isValid = DateTime.TryParseExact(
                dataSortidaString,
                "yyyyMMddHHmm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _);

            if (!isValid)
            {
                return new ValidationResult("Formato inválido: debe ser yyyyMMddHHMM (12 dígitos con fecha y hora válidas).");
            }

            return ValidationResult.Success!;
        }

    }
}