using FastEvalCL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MakeLifeEasierWPF.Convertors
{
    public class EnumCompileerTestToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Tested waarde = (Tested)value;
                switch (waarde)
                {
                    case Tested.NietGetest:
                        return "White";
                        break;
                    case Tested.CompileertNiet:
                        return "Red";
                        break;
                    case Tested.Compileert:
                        return "Green";
                        break;
                    default:
                        return "Yellow";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}