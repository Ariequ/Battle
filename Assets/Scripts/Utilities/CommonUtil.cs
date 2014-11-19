using System;
using System.Text;

public class CommonUtil
{
    public const int ONE_DAY_SECONDS = 86400;

    public static string ParseTimeToString(long duration)
    {
        StringBuilder resultSB = new StringBuilder();
        string resultStr;

        if (duration < ONE_DAY_SECONDS)
        {
            int hour = (int)(duration / 3600);
            duration %= 3600;
            int min = (int)(duration / 60);
            duration %= 60;
            int second = (int)(duration % 60);

            resultSB.Append(hour.ToString() + "hour ");
            if (min != 0)
                resultSB.Append(min.ToString() + "minute ");
            if (second != 0)
                resultSB.Append(second.ToString() + "second ");
            resultStr = resultSB.ToString();
        }
        else
        {
            resultStr = (duration / ONE_DAY_SECONDS).ToString() + "day";
        }

        return resultStr;
    }

	public static int GetNumFromStringEnd(string str, char divider)
	{
		int index = str.IndexOf(divider);
		if (index != -1)
		{
			string numStr = str.Substring(index + 1);
			if (!String.IsNullOrEmpty(numStr))
			{
				return Convert.ToInt32(numStr);
			}
			return 0;
		}
		return 0;
	}
}

