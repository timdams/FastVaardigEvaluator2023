using FastEvalCL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MakeLifeEasierWPF.Convertors
{


    public class EnumToCheckBoxStateConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Tested waarde = (Tested)value;
                switch (waarde)
                {
                    case Tested.NietGetest:
                        return null;
                        break;
                    case Tested.CompileertNiet:
                        return true;
                        break;
                    case Tested.Compileert:
                        return false;
                        break;
                    default:
                        return null;
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
            bool? waardeIn = (bool?)value;
            if (waardeIn.HasValue)
            {
                if (waardeIn.Value)
                    return Tested.CompileertNiet;
                else return Tested.Compileert;
            }
            else return Tested.NietGetest;
        }
    }
}
