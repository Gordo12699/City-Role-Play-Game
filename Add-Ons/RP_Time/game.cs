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
