

using Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace CustomerWpf.Infrastructure.ViewModels
{
    public class CustomerViewModel : ViewModel
    {
        private string _name;
        private string _customerCode;
        private string _cnp;
        private string _address;

        [Required]
        public string CustomerCode
        {
            get => _customerCode;
            set
            {
                _customerCode = value;
                NotifyPropertyChanged();
            }
        }

        [Required]
        public string Name
        {
            get=>_name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        [Required]
        public string CNP
        {
            get => _cnp;
            set
            {
                _cnp = value;
                NotifyPropertyChanged();
            }
        }

        [Required]
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                NotifyPropertyChanged();
            }
        }

        #region Overrides of ViewModel

        protected override string OnValidate(string propertyName)
        {
            if (Name!=null&&Name.Length < 4) return "Customer Name must be greater than 4 characters.";

            return base.OnValidate(propertyName);
        }

        #endregion
    }
}
