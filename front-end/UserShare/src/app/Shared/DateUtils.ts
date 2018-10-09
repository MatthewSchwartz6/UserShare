export class AmericanDateUtils{
    public static formatDate(unformattedDate: string): string {
        let dateArray = unformattedDate.split("T");
        if (dateArray.length == 0) return unformattedDate;
        let formattedYear = this.formatYear(dateArray[0]);
        let formattedTime = this.formatTime(dateArray[1]);
        return formattedTime + " on " + formattedYear;
    }
    private static formatYear(unformattedYear: string): string {
        let unFormattedArray = unformattedYear.split("-");
        let formattedYear = unFormattedArray[1] + "/" + unFormattedArray[2] + "/" + unFormattedArray[0];
        return formattedYear;
    }
    private static formatTime(unformattedTime: string): string {
        let unFormattedArray = unformattedTime.split(":");
        let unFormattedHours = unFormattedArray[0];
        if (unFormattedHours.charAt(0) == '0' || Number(unFormattedHours) <= 12)
        {
            let formattedTime = this.formatMidnightOrNoon(unFormattedArray[0]) + ":" + unFormattedArray[1] + "AM";
            return formattedTime;
        }
        else
        {
            var hoursTens = Number(unFormattedHours);
            hoursTens -= 12;
            let formattedTime = this.formatMidnightOrNoon((hoursTens < 10 ? "0": "") + hoursTens) + ":" + unFormattedArray[1] + "PM";
            return formattedTime;
        }
    }
    private static formatMidnightOrNoon(time: string): string
    {
        if (time == "00") return "12";
        else return time;
    }

}