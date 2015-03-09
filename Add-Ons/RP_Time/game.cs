// --------------------------
// RP Time :: Game
// --------------------------

if (isPackage(RP_Time_Game_Boot))
	deactivatePackage(RP_Time_Game_Boot);

package RP_Time_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		CreateDate();
	
		if (isEventPending(RPDB.scheduleTick))
			Cancel(RPDB.scheduleTick);
		
		RPDB.scheduleTick = schedule(60000 * $RP::pref::game::tick, 0, RPTick);
		setEnvironment("DayCycleEnabled", 1);
		setEnvironment("DayLength", ($RP::pref::game::tick * 24) * 60);
	}
	
	// Shutting game
	function ShutRP()
	{
		if (isEventPending(RPDB.scheduleTick))
			Cancel(RPDB.scheduleTick);
		if (isEventPending(RPDB.keySaveTick))
			Cancel(RPDB.keySaveTick);
		
		Parent::ShutRP();
	}
};
activatePackage(RP_Time_Game_Boot);

// Schedule for tick
function RPTick()
{
	if (!isObject(RPDB))
		return;
	if (isEventPending(RPDB.scheduleTick))
		Cancel(RPDB.scheduleTick);
	
	// Increase hour
	$RP::pref::timeHour++;
	
	if($RP::pref::timeHour == 5)
	{
		//Sync DayCycle with RP Time
		if (!isObject(DayCycle)) 
		{
			error("ERROR: DayCycle does not exist.");
			//return;
		}

		if (DayCycle.dayLength != ($RP::pref::game::tick * 24) * 60) 
		{
			error("ERROR: DayCycle length is not equal to RP Day Length.");
			//return;
		}

		%all = strReplace(getWord(getDateTime(), 1), ":", " ");

		%real = 1 * 3600 + 1 * 60 + (60 - mCeil((getTimeRemaining(RPDB.scheduleTick) / $RP::pref::game::tick) * 0.001));
		%curr = $Sim::Time / (($RP::pref::game::tick * 24) * 60);

		DayCycle.setDayOffset(%real - (%curr - mFloor(%curr)));
	}
	// Increase day
	if ($RP::pref::timeHour > 24)
	{
		$RP::pref::timeHour = 1;
		$RP::pref::timeDay++;
	}
	// Leap year
	%days = $Date::monthDays[$RP::pref::timeMonth];
	if ($RP::pref::timeMonth == 2 && IsLeapYear())
		%days = 29;
	// Increase month
	if ($RP::pref::timeDay > %days)
	{
		$RP::pref::timeDay = 1;
		$RP::pref::timeMonth++;
	}
	// Increase year
	if ($RP::pref::timeMonth > 12)
	{
		$RP::pref::timeMonth = 1;
		$RP::pref::timeYear++;
	}
	
	messageAll('', getClockTime($RP::pref::timeHour, $RP::pref::game::am_pm));
	
	// Check online players
	for (%i = 1; %i <= RPDB.countOnline; %i++)
		CheckTickID(RPDB.listOnline[%i]);
	
	SaveIDTick(0);
	RPDB.scheduleTick = schedule(60000 * $RP::pref::game::tick, 0, RPTick);
}

// Make a player tick go
function CheckTickID(%ID)
{
	RPDB.set(%ID, "exp", RPDB.get(%ID, "exp") + $RP::pref::user::tickExp);
	
	return true;
}

// Save each ID tick
function SaveIDTick(%i)
{
	%i = atoi(%i);
	%ID = RPDB.listOnline[%i];
	%i++;
	RPDB.saveKey(%ID);
	if (%i > RPDB.countOnline)
		return;
	RPDB.keySaveTick = schedule(1000, 0, SaveIDTick, %i);
}
