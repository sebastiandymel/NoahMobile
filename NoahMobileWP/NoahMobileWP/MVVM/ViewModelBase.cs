using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace SEDY.PhoneUIToolkit
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private List<INotifyCanExecuteChanged> commands = new List<INotifyCanExecuteChanged>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }

        public void RaisePropertyChanged()
        {
            var stack = new StackTrace(1, true);
            var property = stack.GetFrame(0)
                .GetMethod().Name
                .Replace("set_", string.Empty)
                .Replace("get_", string.Empty);
            RaisePropertyChanged(property);
        }

        protected void RegisterCommand(INotifyCanExecuteChanged cmd)
        {
            if (commands.Contains(cmd))
            {
                return;
            }
            commands.Add(cmd);
        }

        protected void UnregisterCommand(INotifyCanExecuteChanged cmd)
        {
            if (commands.Contains(cmd))
            {
                commands.Remove(cmd);
            }
        }

        protected void NotifyCommandsCanExecuteChanged()
        {
            foreach (var cmd in commands)
            {
                cmd.RaiseCanExecuteChanged();
            }
        }

        protected virtual void Initialize()
        {
            
        }

        protected virtual void Uninitialize()
        {
            if (commands != null)
            {
                commands.Clear();
                commands = null;
            }
        }
    }
}
