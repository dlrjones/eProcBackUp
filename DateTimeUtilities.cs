using System;

namespace eProcBackUp
{
    class DateTimeUtilities
    {
        public string DateTimeToShortDate(DateTime dt)
        {
            //returns ShortDate format M/d/yyyy
            return dt.ToString("d");
        }

        /// <summary>
        /// Returns DateTime.Now as a short datetime in the form ddmmyyhhmmss
        /// </summary>
        /// <param name="[no params]"></param>
        /// <returns>DateTime.Now as ddmmyyhhmmss</returns>
        public string DateTimeCoded()
        {
            return DateTimeCoded(false);
        }
        /// <summary>
        /// Returns a short datetime in the form ddmmyyhhmmss[xxxxxxx]
        /// </summary>
        /// <param name="withMilliSeconds">True=Include milliseconds</param>
        /// <returns> DateTime.Now as ddmmyy</returns>
        public string DateTimeCoded(bool withMilliSeconds)
        {
            //returns [month day year hours min sec] as a string 
            //withMilliSeconds = true appends the 7 millisecond chars to the end [mmddyyhhmmssxxxxxxx]
            string rtnValu = "";
            string[] date;
            string[] tod;
            string delimitString = " :./";
            char[] delimiter = delimitString.ToCharArray();

            date = DateTime.Now.ToString().Split((delimiter));
            tod = DateTime.Now.TimeOfDay.ToString().Split((delimiter));
            for (int x = 0; x < date.Length - 1; x++)
            {//the date[] has 7 values the last being "AM" or "PM", which we don't want
                if (date[x].Length == 4)
                {//a four digit value is the year
                    char[] year = date[x].ToCharArray();
                    rtnValu += year[2].ToString();//for a four digit year, strip off
                    rtnValu += year[3].ToString();//the last two digits
                }
                else
                {
                    if (date[x].Length == 0)//prepend leading zero's
                        rtnValu += "00";
                    if (date[x].Length == 1)
                        rtnValu += "0";
                    rtnValu += date[x];
                }
            }
            if (withMilliSeconds)
            {
                rtnValu += tod[3].ToString(); //the tod[] has 4 values (h-m-s-ms) so element 3 holds the millisecond
            }

            return rtnValu;
        }


    }
}
