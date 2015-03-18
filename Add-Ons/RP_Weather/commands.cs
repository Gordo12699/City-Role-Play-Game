// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// RP_Weather :: Commands
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

function serverCmdWeather(%client, %type, %amount)
{
	if (!$RP::activated)
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	if(!$RP::pref::Weather)
		return messageClient(%client, '', '\c6Weather is \c0disabled\c6.');
	
	if(%amount !$= "") { %amount = mFloor(%amount); }
	
	if(%amount >= 0 && %type !$= "")
	{
		%radioname = $RP::pref::radioname;
		%radiofreq = $RP::pref::radiofreq;
		
		if(%type $= "clear" || %type $= "")
		{
			if(%amount > 50)
				%amount = 50;
			
			if(%amount > 0)
			{
				%ammount = ((51 - %amount) * 150);
				WeatherSO.generateweather("clear", %ammount);
				%extra = " with a fog intensity of \c3" @ %amount @ "\c6.";
			}
			else
			{
				WeatherSO.generateweather("clear", "0");
				%extra = ".";
			}
			messageAll('','\c6[\c3%1 %2\c6]:\c3 %3 \c6Changed The Weather To,\c3 Clear\c6%4', %radioname, %radiofreq, %client.name, %extra);
			return;
		}
		else if(%type $= "snow" || %type $= "rain")
		{
			if(%amount > 50)
			{
				%amount = 50;
			}
			else if(%amount <= 0)
			{
				WeatherSO.generateWeather("clear", "0");
				messageAll('','\c6[\c3%1 %2\c6]:\c3 %3 \c6Changed The Weather To,\c3 Clear\c6.', %radioname, %radiofreq, %client.name);
				return;
			}

			WeatherSO.generateWeather(%type, %amount);
			messageAll('','\c6[\c3%1 %2\c6]:\c3 %3 \c6Changed The Weather To,\c3 %4 \c6With An Intensity Of\c3 %5\c6.', %radioname, %radiofreq, %client.name, %type, %amount);
			return;

		}
		else
		{
			messageClient(%client, '', '\c6[\c0ERROR\c6]: \c3%1 \c6is not a valid weather type.', %type);
			return;
		}
	}
	else
	{
		messageClient(%client, '', '\c6[\c0ERROR\c6]: Please provide a weather type and a weather intensity.');
		return;
	}
}

// Date overwrite
function ServerCmdDate(%client)
{
	if (!$RP::activated)
		return;
	
	// Day Word Setter
	%time = $RP::pref::timeHour;
	if(%time > 1 && %time <= 6)
	{
		%dayword = "Morning";
	}
	else if(%time >= 7 && %time <= 14)
	{
		%dayword = "Day";
	}
	else if(%time >= 15 && %time <= 19)
	{
		%dayword = "Sunset";
	}
	else if(%time >= 20 || %time <= 1)
	{
		%dayword = "Night";
	}
	messageClient(%client, '', '\c6The date is: \c3%1 \c6(\c3%8\c6)\c3 %2 %3\c6%4 \c3%5\c6, \c3%6 \c6With a \c3%7 \c6forecast', 
	getClockTime($RP::pref::timeHour, $RP::pref::game::am_pm),
	GetDayOfWeek($RP::pref::timeYear, $RP::pref::timeMonth, $RP::pref::timeDay),
	$RP::pref::timeDay,
	getNumberExtra($RP::pref::timeDay),
	$Date::monthName[$RP::pref::timeMonth],
	$RP::pref::timeYear,
	$RP::pref::CurrWeather,
	%dayword);
	
}

function PlayLoopingSound(%sound, %timestoloop, %secs)
{
	for(%cl=0;%cl<clientgroup.getcount();%cl++)
	{
		%client = clientgroup.getobject(%cl);
		if(%client.hasSpawnedOnce)
		{
			if(%timesplayed <= %timestoloop)
			{
				%client.play2d(%sound);
				%looptyme = (%secs * 1000);
				%timesplayed++;
			}
			else
			{
				%timesplayed = 0;
				return;
			}
		}
	}
	WeatherSO.SoundLoop = schedule(%looptyme, false, "PlayLoopingSound", %sound, %timestoloop, %secs);
	return;
}

function ClientPlaySound(%client, %sound, %timestoloop, %secs)
{
	if(%client.hasSpawnedOnce && isObject(%sound) && %timetoloop !$= "" && %secs > 0)
	{
		if(isObject(WeatherSO.ClientSoundLoop))
			cancel(WeatherSO.ClientSoundLoop);
		
		if(%timesplayed <= %timestoloop)
		{
			%client.play2d(%sound);
			if(%timestoloop = -1)
			{
				%timesplayed = 0;
			}
			else
			{
				%timesplayed++;
			}
			%looptyme = (%secs * 1000);
			WeatherSO.ClientSoundLoop = schedule(%looptyme, false, "ClientPlaySound", %client, %sound, %timestoloop, %secs);
		}
		else
		{
			%timesplayed = 0;
			return;
		}
	}
}

function editMap()
{
	if (!$RP::activated)
		return;
	
	if($MapAltered != 1)
	{
		for(%o=0;%o<missiongroup.getcount();%o++)
		{
			%obj = missiongroup.getobject(%o);
			if(isObject(%obj))
			{
				%objname = strlwr(%obj.getclassname());
				if(%objname $= "sun" || %objname $= "sky" || %objname $= "fxsunlight")
				{
					%obj.save("config/server/RP_Core/RP_Weather/" @ %objname @ "_old.cs", 0);
					if($RP::pref::altersky == 1 && %objname $= "sky") { %obj.delete(); } else
					if($RP::pref::altersun == 1 && %objname $= "sun") { %obj.delete(); } else
					if($RP::pref::altersunfx == 1 && %objname $= "fxsunlight") { %obj.delete(); }
				}
			}
		}
		DaySO.getday();
		$MapAltered = 1;
	}
}

function returnmap()
{
	if($MapAltered != 1)
		return;
	// Sky
	if(isObject(Sky))
		Sky.delete();
	if(isFile("config/server/RP_Core/RP_Weather/sky_old.cs"))
		exec("config/server/RP_Core/RP_Weather/sky_old.cs");
	
	// Sun
	if(isObject(Sun))
		Sun.delete();
	if(isFile("config/server/RP_Core/RP_Weather/sun_old.cs"))
		exec("config/server/RP_Core/RP_Weather/sun_old.cs");
	
	// SunFX
	if(isObject(SunFX))
		SunFX.delete();
	if(isFile("config/server/RP_Core/RP_Weather/fxsunlight_old.cs"))
		exec("config/server/RP_Core/RP_Weather/fxsunlight_old.cs");
	
	// Weather
	if(WeatherSO.Soundloop)
		cancel(WeatherSO.Soundloop);
	if(isObject(Precipitation))
		Precipitation.delete();

	$MapAltered = 0;
}
