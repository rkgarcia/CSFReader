using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace CSFExtractor
{
    public partial class CSF
    {
        private string Text = "";
        private const string Moral = "Moral";
        private const string Physical = "Física";

        public string FileName { get; set; }
        public string RFC { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastSurname  { get; set; }
        public string CIF  { get; set; }
        public string Date  { get; set; }
        public string CURP  { get; set; }
        public string Started  { get; set; }
        public string Status  { get; set; }
        public string LastChange  { get; set; }
        public string Type  { get; set; }
        public string Postal  { get; set; }
        public string StreetType  { get; set; }
        public string Street  { get; set; }
        public string Number  { get; set; }
        public string IntNumber  { get; set; }
        public string Colony  { get; set; }
        public string Locality  { get; set; }
        public string Municipality  { get; set; }
        public string State  { get; set; }
        public string Between  { get; set; }
        public string AndBetween  { get; set; }
        public string Email  { get; set; }
        public string AreaCode  { get; set; }
        public string Phone { get; set; }

        public string RegimeCount { get; set; }
        public string Regime1 { get; set; }
        public string RegimeDate1 { get; set; }
        public string Regime2 { get; set; }
        public string RegimeDate2 { get; set; }
        public string Regime3 { get; set; }
        public string RegimeDate3 { get; set; }
        public string Regime4 { get; set; }
        public string RegimeDate4 { get; set; }
        public string Regime5 { get; set; }
        public string RegimeDate5 { get; set; }
        public string Regime6 { get; set; }
        public string RegimeDate6 { get; set; }
        public string Regime7 { get; set; }
        public string RegimeDate7 { get; set; }

        public CSF()
        {
            FileName = "";
            RFC = "";
            FullName = "";
            Name = "";
            Surname = "";
            LastSurname = "";
            CIF = "";
            Date = "";
            CURP = "";
            Started = "";
            Status = "";
            LastChange = "";
            Type = "";
            Postal = "";
            StreetType = "";
            Street = "";
            Number = "";
            IntNumber = "";
            Colony = "";
            Locality = "";
            Municipality = "";
            State = "";
            Between = "";
            AndBetween = "";
            Email = "";
            Phone = "";
            AreaCode = "";
            RegimeCount = "";
            Regime1 = "";
            RegimeDate1 = "";
            Regime2 = "";
            RegimeDate2 = "";
            Regime3 = "";
            RegimeDate3 = "";
            Regime4 = "";
            RegimeDate4 = "";
            Regime5 = "";
            RegimeDate5 = "";
            Regime6 = "";
            RegimeDate6 = "";
            Regime7 = "";
            RegimeDate7 = "";
        }

        public static CSF Load(string FileName) {
            try
            {
                if (!File.Exists(FileName))
                {
                    throw new FileNotFoundException(FileName);
                }
                CSF result = new();
                var sb = new StringBuilder();
                result.FileName = Path.GetFileName(FileName);
                using PdfDocument document = PdfDocument.Open(FileName);
                foreach (Page page in document.GetPages())
                {
                    result.Text += ContentOrderTextExtractor.GetText(page, true);
                }

                result.Text = result.Text.ReplaceLineEndings(" ");
                result.Text = CleanerRegex().Replace(result.Text, "");

                if (!CSFString().IsMatch(result.Text))
                {
                    throw new Exception("El archivo no parece ser una Constancia de Situación Fiscal");
                }

                // Probably we must use more texts to detect if is a Moral CSF
                if (TypeRegex().IsMatch(result.Text))
                {
                    result.Type = Physical;
                    result.Name = NamePhysicalRegex().Match(result.Text).Groups[1].Value.Trim();
                    result.Surname = SurnameRegex().Match(result.Text).Groups[1].Value.Trim();
                    result.LastSurname = LastSurnameRegex().Match(result.Text).Groups[1].Value.Trim();
                }
                else
                {
                    result.Type = Moral;
                    result.Name = NameMoralRegex().Match(result.Text).Groups[1].Value.Trim();
                }

                result.FullName = FullNameRegex().Match(result.Text).Groups[1].Value.Trim();
                result.CIF = CIFRegex().Match(result.Text).Groups[1].Value.Trim();
                result.RFC = RFCRegex().Match(result.Text).Groups[1].Value.Trim();
                result.CURP = CURPRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Date = DateRegex().Match(result.Text).Groups[1].Value.Replace(result.RFC, "").Trim();
                result.Status = StatusRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Started = StartedRegex().Match(result.Text).Groups[1].Value.Trim();
                result.LastChange = LastChangeRegex().Match(result.Text).Groups[1].Value.Trim();
                result.StreetType = StreetTypeRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Street = StreetRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Postal = PostalRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Number = ExternalNumberRegex().Match(result.Text).Groups[1].Value.Trim();
                result.IntNumber = InternalNumberRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Colony = ColonyRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Locality = LocalityRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Municipality = MunicipalityRegex().Match(result.Text).Groups[1].Value.Trim();
                result.State = StateRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Between = BetweenRegex().Match(result.Text).Groups[1].Value.Trim();
                result.AndBetween = AndBetweenRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Email = EmailRegex().Match(result.Text).Groups[1].Value.Trim();
                result.Phone = PhoneRegex().Match(result.Text).Groups[1].Value.Trim();
                result.AreaCode = AreaCodeRegex().Match(result.Text).Groups[1].Value.Trim();
                var regimesRaw = RegimesRegex().Match(result.Text).Groups[1].Value.Trim();
                var regimeNames = DateFormatRegex().Split(regimesRaw).Where(r =>
                {
                    return r.Length > 0;
                }).ToArray();
                var regimeDates = FindDatesRegex().Matches(regimesRaw).ToArray();
                result.RegimeCount = regimeNames.Length.ToString();
                var counter = regimeNames.Length;
                var current = 0;
                if (counter > current)
                {
                    result.Regime1 = regimeNames[current].Trim();
                    result.RegimeDate1 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime2 = regimeNames[current].Trim();
                    result.RegimeDate2 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime3 = regimeNames[current].Trim();
                    result.RegimeDate3 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime4 = regimeNames[current].Trim();
                    result.RegimeDate4 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime5 = regimeNames[current].Trim();
                    result.RegimeDate5 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime6 = regimeNames[current].Trim();
                    result.RegimeDate6 = regimeDates[current].Groups[1].Value.Trim();
                }
                current++;
                if (counter > current)
                {
                    result.Regime7 = regimeNames[current].Trim();
                    result.RegimeDate7 = regimeDates[current].Groups[1].Value.Trim();
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public string GetText()
        {
            return Text;
        }

        // Document cleaner regex
        [GeneratedRegex("Página.{1,2}\\[\\d\\] de \\[\\d\\]")]
        private static partial Regex CleanerRegex();

        // Document detection regexs
        [GeneratedRegex("CONSTANCIA DE SITUACIÓN FISCAL")]
        private static partial Regex CSFString();
        [GeneratedRegex("Primer Apellido:")]
        private static partial Regex TypeRegex();

        [GeneratedRegex("Contribuyentes (.*) Nombre, denominación")]
        private static partial Regex FullNameRegex();
        [GeneratedRegex("Nombre .s.: (.*) Primer")]
        private static partial Regex NamePhysicalRegex();
        [GeneratedRegex("Social: (.*) Régimen Capital:")]
        private static partial Regex NameMoralRegex();
        [GeneratedRegex("Apellido: (.*) Segundo")]
        private static partial Regex SurnameRegex();
        [GeneratedRegex("Segundo Apellido: (.*)  Fecha i")]
        private static partial Regex LastSurnameRegex();
        [GeneratedRegex("idCIF: (.*) VALIDA")]
        private static partial Regex CIFRegex();
        [GeneratedRegex("RFC: (\\w{12,13})")]
        private static partial Regex RFCRegex();
        [GeneratedRegex("CURP: (.{18})")]
        private static partial Regex CURPRegex();
        [GeneratedRegex("Fecha de Emisión (.*) Datos de Iden")]
        private static partial Regex DateRegex();
        [GeneratedRegex("padrón: (\\w*)")]
        private static partial Regex StatusRegex();
        [GeneratedRegex("operaciones: (.*) Estatus")]
        private static partial Regex StartedRegex();
        [GeneratedRegex("estado: (\\d{2} \\w+ \\w+ \\w+ \\d{4})")]
        private static partial Regex LastChangeRegex();
        [GeneratedRegex("Vialidad: (.*) Nombre de V")]
        private static partial Regex StreetTypeRegex();
        [GeneratedRegex("Nombre de Vialidad: (.+) Número Exte")]
        private static partial Regex StreetRegex();
        [GeneratedRegex("\\D{1}(\\d{5}) ")]
        private static partial Regex PostalRegex();
        [GeneratedRegex("Exterior: (.*) Número Interior:")]
        private static partial Regex ExternalNumberRegex();
        [GeneratedRegex("Interior:(.*)Nombre de la Co")]
        private static partial Regex InternalNumberRegex();
        [GeneratedRegex("Colonia:(.*) Nombre de la Loca")]
        private static partial Regex ColonyRegex();
        [GeneratedRegex("Localidad:(.*)Nombre del")]
        private static partial Regex LocalityRegex();
        [GeneratedRegex("Territorial:(.*)Nombre de la Entidad")]
        private static partial Regex MunicipalityRegex();
        [GeneratedRegex("Federativa:(.*)Entre Ca")]
        private static partial Regex StateRegex();
        [GeneratedRegex("Entre Calle:(.*)Y Calle:")]
        private static partial Regex BetweenRegex();
        [GeneratedRegex("Y Calle:(.*)Correo")]
        private static partial Regex AndBetweenRegex();
        [GeneratedRegex("Electrónico:(.*@.+) Tel")]
        private static partial Regex EmailRegex();
        [GeneratedRegex("Número: (\\d+).?Estado")]
        private static partial Regex PhoneRegex();
        [GeneratedRegex("Lada: (\\d+).?Número")]
        private static partial Regex AreaCodeRegex();
        [GeneratedRegex(" Régimen Fecha Inicio Fecha Fin \\d?(.*?)  (Obligaciones|Sus datos)")]
        private static partial Regex RegimesRegex();
        [GeneratedRegex("\\d{2}/\\d{2}/\\d{4}")]
        private static partial Regex DateFormatRegex();
        [GeneratedRegex("(\\d{2}/\\d{2}/\\d{4})")]
        private static partial Regex FindDatesRegex();
    }
}
