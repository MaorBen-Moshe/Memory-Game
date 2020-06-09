using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float r_MinValue;
        private readonly float r_MaxValue;

        public ValueOutOfRangeException(
                                        float i_MinValue,
                                        float i_MaxValue,
                                        string i_Message = null,
                                        Exception i_InnerException = null)
        : base(
            string.Format(
                   format: @"{0}
You have exceeded the range of values {1} : {2}",
                   i_Message,
                   i_MinValue,
                   i_MaxValue), 
            i_InnerException)
        {
            r_MinValue = i_MinValue;
            r_MaxValue = i_MaxValue;
        }
    }
}
