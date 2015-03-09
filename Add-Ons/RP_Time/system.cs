// --------------------------
// RP Time :: System
// --------------------------

// Adjust clock time
// Good for different places for times
function getClockTime(%time, %am_pm, %a)
{
	%minutes = getMinutes();
	if (%minutes <= 9)
		%minutes = "0" @ %minutes;
	%minutes = (%a) ? "00" : %minutes;
	if (%am_pm)
	{
		// English time
		%type = (%time > 12) ? "PM" : "AM";
		%hour = (%time > 12) ? %time - 12 : %time;
		
		// Send time
		return "\c3" @ %hour @ ":" @%minutes SPC %type;
	}
	else
		return "\c3" @ %time @ ":" @%minutes;
}

// Get minutes
function getMinutes()
{
	return (isEventPending(RPDB.scheduleTick)) ? 60 - mCeil((getTimeRemaining(RPDB.scheduleTick) / $RP::pref::game::tick) * 0.001) : 0;
}

// Create date
function CreateDate()
{
	// Dates
	$Date::monthName[1] = "January";
	$Date::monthName[2] = "February";
	$Date::monthName[3] = "March";
	$Date::monthName[4] = "April";
	$Date::monthName[5] = "May";
	$Date::monthName[6] = "June";
	$Date::monthName[7] = "July";
	$Date::monthName[8] = "August";
	$Date::monthName[9] = "September";
	$Date::monthName[10] = "October";
	$Date::monthName[11] = "November";
	$Date::monthName[12] = "December";

	$Date::monthDays[1] = 31;
	$Date::monthDays[2] = 28;
	$Date::monthDays[3] = 31;
	$Date::monthDays[4] = 30;
	$Date::monthDays[5] = 31;
	$Date::monthDays[6] = 30;
	$Date::monthDays[7] = 31;
	$Date::monthDays[8] = 31;
	$Date::monthDays[9] = 30;
	$Date::monthDays[10] = 31;
	$Date::monthDays[11] = 30;
	$Date::monthDays[12] = 31;
	
	// Day-a-week
	// Centuries table
	$DayWeek::century[0] = 0;
	$DayWeek::century[1] = 6;
	$DayWeek::century[2] = 4;
	$DayWeek::century[3] = 2;

	// Months table
	$DayWeek::month[1] = 0;
	$DayWeek::month[2] = 3;
	$DayWeek::month[3] = 3;
	$DayWeek::month[4] = 6;
	$DayWeek::month[5] = 1;
	$DayWeek::month[6] = 4;
	$DayWeek::month[7] = 6;
	$DayWeek::month[8] = 2;
	$DayWeek::month[9] = 5;
	$DayWeek::month[10] = 0;
	$DayWeek::month[11] = 3;
	$DayWeek::month[12] = 5;
	// Leap year
	$DayWeek::month[13] = 6;
	$DayWeek::month[14] = 2;

	// Days table
	$DayWeek::day[0] = "Sunday";
	$DayWeek::day[1] = "Monday";
	$DayWeek::day[2] = "Tuesday";
	$DayWeek::day[3] = "Wednesday";
	$DayWeek::day[4] = "Thursday";
	$DayWeek::day[5] = "Friday";
	$DayWeek::day[6] = "Saturday";
}

// Get the day of the week
function GetDayOfWeek(%year, %month, %day)
{
	%century = mFloor(%year / 100) + 1;
	%b = %year - ((%century - 1) * 100);
	// Leap year
	if ((%b % 4) == 0 && %month <= 2)
		%month += 12;
	%a = $DayWeek::century[(%century % 4)];
	%c = mFloor(%b / 4);
	%d = $DayWeek::month[%month];
	%finit = %a + %b + %c + %d + %day;
	%finit = %finit % 7;
	return $DayWeek::day[%finit];
}

// Leap year
function IsLeapYear()
{
	return (($RP::pref::timeYear % 4 == 0 && $RP::pref::timeYear % 100 != 0) || $RP::pref::timeYear % 400 == 0);
}
