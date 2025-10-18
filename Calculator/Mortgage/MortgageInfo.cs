using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Mortgage;
public class MortgageInfo : ObservableObject
{
    public MortgageInfo()
    {
        DownPaymentPercent = 20m;
        Ammortization = 25;
        PaymentsPerYear = 12;
        AnnualInterestRate = 5.0m;
        StartDate = DateOnly.FromDateTime(DateTime.Now);
        TermInYears = 5;
        TotalCost = 400000m;
    }

    #region Properties

    private decimal _TotalCost;

    public decimal TotalCost
    {
        get { return _TotalCost; }
        set { 
            _TotalCost = value;
            //OnPropertyChanged(nameof(TotalCost));
            OnPropertyChanged(nameof(InitialPrincipal));
            OnPropertyChanged(nameof(DownPayment));
            OnPropertyChanged(nameof(MonthlyPayment));
        }
    }

    private decimal _DownPaymentPercent = 20m;
    private decimal _AnnualInterestRate;
    private int _Ammortization;

    public decimal DownPaymentPercent
    {
        get { return _DownPaymentPercent; }
        set { 
            if (value < 1m)
                value = 1m;
            else if (value > 99m)
                value = 99m;
            _DownPaymentPercent = value;
            //OnPropertyChanged(nameof(DownPaymentPercent));
            OnPropertyChanged(nameof(DownPayment));
            OnPropertyChanged(nameof(InitialPrincipal));
            OnPropertyChanged(nameof(MonthlyPayment));
        }
    }

    public decimal AnnualInterestRate
    {
        get => _AnnualInterestRate;
        set
        {
            _AnnualInterestRate = value;
            OnPropertyChanged(nameof(MonthlyPayment));
        }
    }
    public int Ammortization
    {
        get => _Ammortization;
        set
        {
            _Ammortization = value;
            OnPropertyChanged(nameof(MonthlyPayment));
        }
    }
    public int PaymentsPerYear { get; set; }

    public DateOnly StartDate { get; set; }

    public int TermInYears { get; set; }
    #endregion

    #region Derived Props
    public decimal InitialPrincipal
    {
        get { return TotalCost * (1 - DownPaymentFraction); }
        set { TotalCost = value / (1 - DownPaymentFraction); }
    }
    public decimal DownPayment
    {
        get { return TotalCost * DownPaymentFraction; }
        set { DownPaymentPercent = 100m * (value / TotalCost); }
    }
    public decimal DownPaymentFraction
    {
        get { return DownPaymentPercent / 100m; }
    }
    public DateOnly EndDate
    {
        get { return StartDate.AddYears(Ammortization); }
    }
    public decimal MonthlyPayment
    {
        get
        {
            decimal monthlyRate = ( AnnualInterestRate / 100m ) / PaymentsPerYear;
            int totalPayments = Ammortization * PaymentsPerYear;
            decimal numerator = monthlyRate * (decimal)Math.Pow((double)( 1 + monthlyRate ), totalPayments);
            decimal denominator = (decimal)( Math.Pow((double)( 1 + monthlyRate ), totalPayments) - 1 );
            return InitialPrincipal * ( numerator / denominator );
        }
    }
    #endregion

    #region testing values


    

    #endregion

    #region Subs


    #endregion
}
