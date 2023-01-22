using System;

namespace Utils.CustomNumbers
{
    public interface ICustomNumber
    {
        public event Action Changed;

        public string GetFormatedString(string format);
    }
}