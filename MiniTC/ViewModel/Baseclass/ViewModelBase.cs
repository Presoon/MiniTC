using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace MiniTC.ViewModel.Baseclass
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //zdarzenie informujące o zmiane własności w obiekcie ViewModelu
        public event PropertyChangedEventHandler PropertyChanged;

        //metoda zgłaszjąca zmiany w własościach podanych jako argumenty
        protected void onPropertyChanged(params string[] namesOfProperties)
        {
            //jeśli ktoś obserwuje zdarzenie PropertyChanged
            if (PropertyChanged != null)
            {
                //wywołanie zdarzenia dla wszsytkich zgłoszonych do aktualizacji własności
                //w ten sposób powiadamiamy widok o zmianie stanu własności
                //w modelu widoku
                foreach (var prop in namesOfProperties)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }

        protected bool SetField<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName != null");
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            onPropertyChanged(propertyName);
            return true;
        }
    }
}