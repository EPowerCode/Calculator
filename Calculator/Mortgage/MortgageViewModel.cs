using ControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Mortgage;
public class MortgageViewModel : ObservableObject
{
    public MortgageInfo Mortgage { get; set; }
    public MortgageViewModel()
    {
        Mortgage = new MortgageInfo();
    }

}
