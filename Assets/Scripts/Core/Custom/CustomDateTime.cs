using System;

namespace Core.Custom
{
    public class CustomDateTime
    {
        public event Action<DateTime> DateTimeChanged;
        public DateTime DateTime => _dateTime;
        
        private DateTime _dateTime;
        
        public CustomDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        
        public void AddDays(int days)
        {
            _dateTime = _dateTime.AddDays(days);
            DateTimeChanged?.Invoke(_dateTime);
        }
    }
}