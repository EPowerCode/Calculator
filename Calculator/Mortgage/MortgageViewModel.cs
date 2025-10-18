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
        Mortgage.PropertyChanged += Mortgage_PropertyChanged;
        _paymentNumber = 1;
    }

    private void Mortgage_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Forward relevant changes so bindings update
        if (e.PropertyName == nameof(Mortgage.InitialPrincipal) ||
            e.PropertyName == nameof(Mortgage.MonthlyPayment) ||
            e.PropertyName == nameof(Mortgage.Ammortization) ||
            e.PropertyName == nameof(Mortgage.PaymentsPerYear) ||
            e.PropertyName == nameof(Mortgage.TotalCost))
        {
            OnPropertyChanged(nameof(MaxPaymentNumber));
            OnPropertyChanged(nameof(RemainingPrincipal));
            OnPropertyChanged(nameof(TotalInterestPaid));
        }
    }

    private int _paymentNumber;
    public int PaymentNumber
    {
        get => _paymentNumber;
        set
        {
            int max = MaxPaymentNumber;
            if (value < 0) value = 0;
            if (value > max) value = max;
            if (_paymentNumber != value)
            {
                _paymentNumber = value;
                OnPropertyChanged(nameof(PaymentNumber));
                OnPropertyChanged(nameof(RemainingPrincipal));
                OnPropertyChanged(nameof(TotalInterestPaid));
            }
        }
    }

    public int MaxPaymentNumber => Mortgage.Ammortization * Mortgage.PaymentsPerYear;

    public decimal RemainingPrincipal
    {
        get
        {
            int k = PaymentNumber;
            decimal P = Mortgage.InitialPrincipal;
            int n = Mortgage.Ammortization * Mortgage.PaymentsPerYear;
            if (n <= 0) return 0m;
            decimal r = (Mortgage.AnnualInterestRate / 100m) / Mortgage.PaymentsPerYear;
            if (k <= 0) return P;
            if (k >= n) return 0m;
            if (r == 0m)
            {
                decimal rem = P - k * Mortgage.MonthlyPayment;
                return rem < 0 ? 0 : rem;
            }
            double rd = (double)r;
            double pow_n = Math.Pow(1 + rd, n);
            double pow_k = Math.Pow(1 + rd, k);
            decimal powN = (decimal)pow_n;
            decimal powK = (decimal)pow_k;
            decimal numerator = powN - powK;
            decimal denominator = powN - 1m;
            if (denominator == 0m) return 0m;
            decimal Bk = P * (numerator / denominator);
            return Bk;
        }
    }

    public decimal TotalInterestPaid
    {
        get
        {
            decimal totalPaid = PaymentNumber * Mortgage.MonthlyPayment;
            decimal principalPaid = Mortgage.InitialPrincipal - RemainingPrincipal;
            decimal interest = totalPaid - principalPaid;
            return interest < 0 ? 0 : interest;
        }
    }

}
