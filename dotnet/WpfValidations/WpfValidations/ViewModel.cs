using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace WpfValidations
{
    public class ViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Jmeno {0} je povinne")]
        public string Name { get; set; }

        public int Test1 { get; set; }

        [XmlAttribute]
        public string Test2 { get; set; }

        [Required(ErrorMessage = "Ulice")]
        [MinLength(5)]
        [MaxLength(2)]
        public string Surname { get; set; }

        public int ID { get; set; }

        public ICommand ValidateCommand { get; set; }

        public ViewModel()
        {
            Name = "Jmeno";
            Surname = "aaa";

            ValidateCommand = new DelegateCommand(ValidateViewModel);
        }

        private void ValidateViewModel()
        {
            base.Validate();
        }
    }

    public abstract class BaseViewModel
    {
        [Required]
        public string BaseName { get; set; }

        [XmlAttribute]
        public string BaseSurname { get; set; }

        public event EventHandler<DataValidationCompletedEventArgs> ValidationCompleted;

        protected virtual void Validate()
        {
            var props = from p in this.GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(ValidationAttribute), true)
                        where attr.Length > 0
                        select new { PropertyName = p.Name, PropertyValue = p.GetValue(this), Attributes = attr };

            var msg = new StringBuilder(1000);

            foreach (var prop in props)
            {
                var context = new ValidationContext(this)
                {
                    MemberName = prop.PropertyName
                };

                var results = new Collection<ValidationResult>();

                foreach (ValidationAttribute attr in prop.Attributes)
                {
                    var validationAttributes = new[] { attr };
                    var isValid = Validator.TryValidateValue(prop.PropertyValue, context, results, validationAttributes);
                    if (!isValid)
                    {
                        msg.AppendLine(string.Format("{0}: {1} - {2} - {3}\n", results[0].ErrorMessage, prop.PropertyName, prop.PropertyValue, attr));
                    }
                }
            }

            if (ValidationCompleted != null)
            {
                ValidationCompleted(this, new DataValidationCompletedEventArgs(msg.ToString()));
            }
        }
    }

    public class DataValidationCompletedEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public DataValidationCompletedEventArgs(string message)
        {
            Message = message;
        }
    }
}
