// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// Project: RP_Weather
// Author: JJstorm
// Description: Weather For RP_Core
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// Table of Contents
// 1. Package
// 2. WeatherSO
// 3. DaySO
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

if(!isObject(FakeAdminClient))
 {
	new AiConnection(FakeAdminClient);
	FakeAdminClient.isAdmin = 1;
	FakeAdminClient.isSuperAdmin = 1;
}

// ============================================================
// Section 1 : Package
// ============================================================

if (isPackage(RP_Weather))
	deactivatePackage(RP_Weather);

package RP_Weather
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
		return;

		Parent::BootRP();

		//editmap();
	}

	function ShutRP()
	{
		if (!$RP::activated)
		return;

		Parent::ShutRP();
		
		//returnmap();
	}

	function RPTick()
	{
		parent::RPTick();
		
		DaySO.getDay();
		if($RP::pref::timeHour == 2)
		{
			WeatherSO.getweather();
		}
	}
	
	function changeMap(%path)
	{
		$MapAltered = 0;
		Parent::changeMap(%path);
	}
	
	function servercmdforcetick(%client)
	{
		if(!DaySO.timepassing)
		{
			Parent::servercmdforcetick(%client);
		}
	}
};
RPRegPackage(RP_Weather);

// ============================================================
// Section 2 : WeatherSO
// ============================================================

function WeatherSO::loadForecast(%so)
{
	// Rain Probability
	%so.rainProb[1] = $RP::pref::RainMonth1; // January
	%so.rainProb[2] = $RP::pref::RainMonth2; // February
	%so.rainProb[3] = $RP::pref::RainMonth3; // March
	%so.rainProb[4] = $RP::pref::RainMonth4; // April
	%so.rainProb[5] = $RP::pref::RainMonth5; // May
	%so.rainProb[6] = $RP::pref::RainMonth6; // June
	%so.rainProb[7] = $RP::pref::RainMonth7; // July
	%so.rainProb[8] = $RP::pref::RainMonth8; // August
	%so.rainProb[9] = $RP::pref::RainMonth9; // September
	%so.rainProb[10] = $RP::pref::RainMonth10; // October
	%so.rainProb[11] = $RP::pref::RainMonth11; // November
	%so.rainProb[12] = $RP::pref::RainMonth12; // December
	
	// Snow Probability
	%so.snowProb[1] = $RP::pref::SnowMonth1; // January
	%so.snowProb[2] = $RP::pref::SnowMonth2; // February
	%so.snowProb[3] = $RP::pref::SnowMonth3; // March
	%so.snowProb[4] = $RP::pref::SnowMonth4; // April
	%so.snowProb[5] = $RP::pref::SnowMonth5; // May
	%so.snowProb[6] = $RP::pref::SnowMonth6; // June
	%so.snowProb[7] = $RP::pref::SnowMonth7; // July
	%so.snowProb[8] = $RP::pref::SnowMonth8; // August
	%so.snowProb[9] = $RP::pref::SnowMonth9; // September
	%so.snowProb[10] = $RP::pref::SnowMonth10; // October
	%so.snowProb[11] = $RP::pref::SnowMonth11; // November
	%so.snowProb[12] = $RP::pref::SnowMonth12; // December
}

function WeatherSO::getweather(%so)
{
	if(!$RP::pref::Weather)
		return;

	%month = $RP::pref::timeMonth;
	
	%snowProb = %so.snowProb[%month];
	%rainProb = %so.rainProb[%month];
	%clearProb = 100 - (%so.snowProb[%month] + %so.rainProb[%month]);
	
	if(%snowProb > 0)
		%snowPow = getRandom(0, %snowProb);
	if(%rainProb > 0)
		%rainPow = getRandom(0, %rainProb);
	if(%clearProb > 0)
		%clearPow = getRandom(0, %clearProb);
	
	if(%snowPow > %rainPow && %snowPow > %clearPow)
		%forecast = "snow";
	else if(%rainPow > %snowPow && %rainPow > %clearPow)
		%forecast = "rain";
	else
		%forecast = "";
	
	// If the second number from last is a "1" (e.g. 12 or 516), the suffix will always be "th"
	if(strlen($RP::pref::timeDay) > 1 && getSubStr($RP::pref::timeDay, (strlen($RP::pref::timeDay) - 2), 1) $= "1")
	{
		%suffix = "th";
	}
	// If not, it can either be "st," "nd," "rd," or "th," depending on the last numeral.
	else
	{
		switch(getSubStr($RP::pref::timeDay, (strlen($RP::pref::timeDay) - 1), 1))
		{
			case 1: %suffix = "st";
			case 2: %suffix = "nd";
			case 3: %suffix = "rd";
			default: %suffix = "th";
		}
	}
	
	%day = GetDayOfWeek($RP::pref::timeYear, $RP::pref::timeMonth, $RP::pref::timeDay);
		
	if(%forecast !$= "")
	{
		%intensity = getRandom(0, 50);
		
		if(%forecast $= $RP::pref::CurrWeather) {
			%verb = "remains";
		}
		else
		{
			$RP::pref::CurrWeather = %forecast;
			%verb = "is";
		}
		
		%intensity = getRandom(5, 15);
		
		switch$(%forecast)
		{
			case "snow":
				//%so.generateWeather("snow", %intensity);
				serverCmdEnvGui_SetVar(FakeAdminClient,"SkyIdx",1);
				$RP::pref::CurrWeather = "snow";
				messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 (%5) of year \c3%3\c6, ...', $Date::monthName[$RP::pref::timeMonth], $RP::pref::timeDay, $RP::pref::timeYear, %suffix, %day);
				messageAll('', '\c6 - Today\'s forecast %1 Snowy.', %verb);
				cancel($Thunder);
				$Thunder = "";
				missionGroup.remove($Lightning);
				$Lightning.delete();
			case "rain":
				//%so.generateWeather("rain", %intensity);
				serverCmdEnvGui_SetVar(FakeAdminClient,"SkyIdx",6);
				$RP::pref::CurrWeather = "rain";
				messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 (%5) of year \c3%3\c6, ...', $Date::monthName[$RP::pref::timeMonth], $RP::pref::timeDay, $RP::pref::timeYear, %suffix, %day);
				messageAll('', '\c6 - Today\'s forecast %1 Rainy.', %verb);
				if(getRandom(1, 4) == 3)
				{
					serverCmdToggleThunder(FakeAdminClient);
				}
			case "clear":
				//%so.generateWeather("clear", 0);
				serverCmdEnvGui_SetVar(FakeAdminClient,"SkyIdx",3);
				$RP::pref::CurrWeather = "clear";
				messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 (%5) of year \c3%3\c6, ...', $Date::monthName[$RP::pref::timeMonth], $RP::pref::timeDay, $RP::pref::timeYear, %suffix, %day);
				messageAll('', "\c6 - Today's forecast is Clear.");
				cancel($Thunder);
				$Thunder = "";
				missionGroup.remove($Lightning);
				$Lightning.delete();
			default:
				//%so.generateWeather("clear", 0);
				serverCmdEnvGui_SetVar(FakeAdminClient,"SkyIdx",12);
				$RP::pref::CurrWeather = "clear";
				messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 (%5) of year \c3%3\c6, ...', $Date::monthName[$RP::pref::timeMonth], $RP::pref::timeDay, $RP::pref::timeYear, %suffix, %day);
				messageAll('', '\c6 - Today\'s forecast %1 \c0INVALID\c6.', %verb);
				cancel($Thunder);
				$Thunder = "";
				missionGroup.remove($Lightning);
				$Lightning.delete();
		}
	}
	else
	{
		
	
		messageAll('', '\c6Today, on \c3%1\c6 the \c3%2\c6%4 (%5) of year \c3%3\c6, ...', $Date::monthName[$RP::pref::timeMonth], $RP::pref::timeDay, $RP::pref::timeYear, %suffix, %day);
		messageAll('', "\c6 - Today's forecast is Clear.");
		serverCmdEnvGui_SetVar(FakeAdminClient,"SkyIdx",3);
		cancel($Thunder);
		$Thunder = "";
		missionGroup.remove($Lightning);
		$Lightning.delete();
		//%so.generateWeather("clear", 0);
	}
}

function WeatherSO::generateWeather(%so, %weather, %intensity)
{
	if(!$RP::pref::Weather)
		return;

	%weather = strlwr(%weather);
	$RP::pref::CurrWeather = %weather;
	%intensity = mFloor(%intensity);
	
	if(isObject(Precipitation))
		Precipitation.delete();
	
	if(WeatherSO.Soundloop)
		cancel(WeatherSO.Soundloop);
	
	if(%weather !$= "" && %intensity > 0)
	{
		switch$(%weather)
		{
			case "snow":
				%loop = (($RP::pref::game::tick * 60000) * (24 - $RP::pref::timeHour) / 8000);
				PlayLoopingSound("SnowSound", %loop, 8);
				%db = "SnowA";
				%mutliplier = 0.15;
			case "rain":
				%loop = (($RP::pref::game::tick * 60000) * (24 - $RP::pref::timeHour) / 16000);
				PlayLoopingSound("RainSound", %loop, 16);
				%db = "RP_Weather_Rain";
				%mutliplier = 0.15;
			case "clear":
				%r = getword($Sky.SkySolidColor, 0);
				%g = getword($Sky.SkySolidColor, 1);
				%b = getword($Sky.SkySolidColor, 2);
				%fr = getword($Sky.FogColor, 0);
				%fg = getword($Sky.FogColor, 1);
				%fb = getword($Sky.FogColor, 2);
				$RP::pref::FogIntensity = %intensity;
				DaySO.setsky(%r,%g,%b,%fr,%fg,%fb,%intensity);
			default:
				%db = "SnowA";
				%mutliplier = 0.15;
		}
		
		%intensity = %intensity * %mutliplier;
		// Default Properties
		%numDrops = 400 * %intensity;
		%minSpeed = 0.5 * %intensity;
		%maxSpeed = 1 * %intensity;
		%minMass = 0.75 * %intensity;
		%maxMass = 0.85 * %intensity;
		%maxTurbulence = 0.1 * %intensity;
		%turbulenceSpeed = 0.5 * %intensity;
		
		new Precipitation(Precipitation)
		{
			dataBlock = %db;
			numDrops = %numDrops;
			
			position = "191.844 621.068 566.484";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			doCollision = "1";
			
			boxWidth = "100";
			boxHeight = "100";
			
			minSpeed = %minSpeed;
			maxSpeed = %maxSpeed;
			
			minMass = %minMass;
			maxMass = %maxMass;
			
			// Beta
			useTurbulence = "true";
			maxTurbulence = %maxTurbulence;
			turbulenceSpeed = %turbulenceSpeed;
			// Beta
			rotateWithCamVel = "true";
		};
	}
	else
	{
		DaySO.getday();
		return;
	}
}

if(!isObject(WeatherSO))
{
	new scriptObject(WeatherSO) { };
	WeatherSO.loadForecast();

}

// ============================================================
// Section 3 : DaySO
// ============================================================
function DaySO::getday(%so)
{
	
	DaySO.generateday($RP::pref::timeHour);
}

function DaySO::setsun(%so,%r,%g,%b,%scale)
{
	if($RP::pref::altersun != 1)
		return;
	
	if(isObject(Sun))
		Sun.delete();
	
	%pos = "10.0045 -444.123" SPC (20.5 * $RP::pref::timeHour);
	
	$Sun = new Sun(Sun)
	{
		color = %r SPC %g SPC %b SPC "1";
		ambient = %r SPC %g SPC %b SPC "1";
		scale = %scale SPC %scale SPC %scale;
		locked = "false";
		direction = "0.57735 0.57735 -0.57735";
		position = %pos;
		canSaveDynamicFields = "1";
		rotation = "1 0 0 0";
	};
}

function DaySO::setSunFX(%so,%path,%r,%g,%b,%scale)
{
	if($RP::pref::altersunfx != 1)
		return;
	
	if(isObject(Sunfx))
		Sunfx.delete();
	
	%tickele = $RP::pref::game::tick * 1207;
	%time = $RP::pref::timeHour;

	$Sunfx = new fxSunLight(SunFX)
	{
		position = "121.979 -83.8798 51.8515";
		rotation = "0 0 0 0";
		scale = %scale SPC %scale SPC %scale;
		Enable = "1";
		Locked = "false";
		LocalFlareBitmap = %path;
		RemoteFlareBitmap = "None";
		SunAzimuth = "100";
		SunElevation = %tickele;
		LockToRealSun = "1";
		FlareTP = "0";
		Colour = %r SPC %g SPC %b SPC "1.000000";
		Brightness = "0.8";
		FlareSize = "2";
		FadeTime = "0.1";
		BlendMode = "0";
		AnimColour = "0";
		AnimBrightness = "0";
		AnimRotation = "0";
		AnimSize = "0";
		AnimAzimuth = "0";
		AnimElevation = "1";
		LerpColour = "1";
		LerpBrightness = "1";
		LerpRotation = "1";
		LerpSize = "1";
		LerpAzimuth = "1";
		LerpElevation = "1";
		LinkFlareSize = "0";
		SingleColourKeys = "1";
		MinColour = "0.000000 0.000000 0.000000 1.000000";
		MaxColour = %r SPC %g SPC %b SPC "1.000000";
		MinBrightness = "0";
		MaxBrightness = "1";
		MinRotation = "0";
		MaxRotation = "359";
		minSize = "0.5";
		maxSize = "1";
		MinAzimuth = "0";
		MaxAzimuth = "359";
		MinElevation = "-30";
		MaxElevation = "210";
		RedKeys = "AZA";
		GreenKeys = "AZA";
		BlueKeys = "AZA";
		BrightnessKeys = "AZA";
		RotationKeys = "AZA";
		SizeKeys = "AZA";
		AzimuthKeys = "AZ";
		ElevationKeys = "AZ";
		ColourTime = %time;
		BrightnessTime = %time;
		RotationTime = %time;
		SizeTime = %time;
		AzimuthTime = %time;
		ElevationTime = %tickele;
	};
}

function DaySO::setSky(%so,%r,%g,%b,%fr,%fg,%fb,%fog)
{
	if($RP::pref::altersky != 1)
		return;
	
	%VD = %fog;
	%FD = mFloor(%VD/2);
	
	if($RP::pref::timeHour >= 20 || $RP::pref::timeHour == 1) { %path = "base/data/skies/Sky_Spooky1/resource.dml"; %usesky = "1";} else { %path = "Add-Ons/RP_Weather/textures/resource.dml"; %usesky = "0";}
	
	if(isObject(Sky)) { Sky.delete(); }
	$Sky = new Sky(Sky)
	{
		position = "336 136 0";
		rotation = "1 0 0 0";
		scale = "1 1 1";
		materialList = %path;
		cloudHeightPer[0] = "0.349971";
		cloudHeightPer[1] = "0.3";
		cloudHeightPer[2] = "0.199973";
		cloudSpeed1 = "0.0005";
		cloudSpeed2 = "0.001";
		cloudSpeed3 = "0.0003";
		visibleDistance = %VD;
		fogDistance = %FD;
		fogColor = %fr SPC %fg SPC %fb SPC "1.000000";
		fogStorm1 = "0";
		fogStorm2 = "0";
		fogStorm3 = "0";
		fogVolume1 = "0 0 0";
		fogVolume2 = "0 0 0";
		fogVolume3 = "0 0 0";
		fogVolumeColor1 = "128.000000 128.000000 128.000000 128.000000";
		fogVolumeColor2 = "128.000000 128.000000 128.000000 128.000000";
		fogVolumeColor3 = "128.000000 128.000000 128.000000 128.000000";
		windVelocity = "1 1 0";
		windEffectPrecipitation = "1";
		SkySolidColor = %r SPC %g SPC %b SPC "1.000000";
		useSkyTextures = %usesky;
		renderBottomTexture = "0";
		noRenderBans = "0";
	};
}
function DaySO::generateday(%so, %timehour)
{
	if($RP::pref::FogIntensity > 0 && strlwr($RP::pref::CurrWeather) $= "clear")
	{
		%fog = $RP::pref::FogIntensity;
	}
	else if(%timehour > 12)
	{
		%fog = (25 - %timehour) * 110;
	}
	else
	{
		%fog = %timehour * 110;
	}
	
	switch$(%timehour)
	{
		case 1:
			DaySO.setSun(0.1, 0.1, 0.1, 1);
			DaySO.setSky(0, 0, 0, 0.1, 0.1, 0.1, %fog);
		case 2:
			if($RP::pref::FogIntensity > 0)
				$RP::pref::FogIntensity = "0";
			DaySO.setSunFX("./textures/sunflare.png",1,1,1,1);
			DaySO.setSun(0.25, 0.25, 0.25, 1);
			DaySO.setSky(0.25, 0.25, 0.25, 0.15, 0.15, 0.15, %fog);
		case 3:
			DaySO.setSun(0.4, 0.4, 0.5, 1);
			DaySO.setSky(0.4, 0.4, 0.5, 0.3, 0.3, 0.3, %fog);
		case 4:
			DaySO.setSun(0.5, 0.5, 0.6, 1);
			DaySO.setSky(0.5, 0.5, 0.6, 0.4, 0.4, 0.4, %fog);
		case 5:
			DaySO.setSun(0.55, 0.55, 0.65, 1);
			DaySO.setSky(0.55, 0.55, 0.65, 0.45, 0.45, 0.45, %fog);
		case 6:
			DaySO.setSun(0.6, 0.6, 0.7, 1);
			DaySO.setSky(0.6, 0.6, 0.7, 0.5, 0.5, 0.5, %fog);
		case 7:
			DaySO.setSun(0.65, 0.65, 0.75, 1);
			DaySO.setSky(0.65, 0.65, 0.75, 0.55, 0.55, 0.55, %fog);
		case 8:
			DaySO.setSun(0.7, 0.7, 0.8, 1);
			DaySO.setSky(0.7, 0.7, 0.8, 0.6, 0.6, 0.6, %fog);
		case 9:
			DaySO.setSun(0.75, 0.75, 0.85, 1);
			DaySO.setSky(0.75, 0.75, 0.85, 0.7, 0.7, 0.7, %fog);
		case 10:
			DaySO.setSun(0.8, 0.8, 0.9, 1);
			DaySO.setSky(0.8, 0.8, 0.9, 0.7, 0.7, 0.7, %fog);
		case 11:
			DaySO.setSun(0.85, 0.85, 0.95, 1);
			DaySO.setSky(0.85, 0.85, 0.95, 0.8, 0.8, 0.8, %fog);
		case 12:
			DaySO.setSun(0.9, 0.9, 1, 1);
			DaySO.setSky(0.9, 0.9, 1, 0.9, 0.9, 0.9, %fog);
		case 13:
			DaySO.setSun(0.85, 0.8, 0.8, 1);
			DaySO.setSky(0.85, 0.8, 0.8, 0.7, 0.7, 0.7, %fog);
		case 14:
			DaySO.setSun(0.8, 0.7, 0.7, 1);
			DaySO.setSky(0.8, 0.7, 0.7, 0.7, 0.7, 0.7, %fog);
		case 15:
			DaySO.setSun(0.8, 0.6, 0.6, 1);
			DaySO.setSky(0.8, 0.6, 0.6, 0.6, 0.6, 0.6, %fog);
		case 16:
			DaySO.setSun(0.75, 0.5, 0.5, 1);
			DaySO.setSky(0.75, 0.5, 0.5, 0.5, 0.5, 0.5, %fog);
		case 17:
			DaySO.setSun(0.7, 0.4, 0.3, 1);
			DaySO.setSky(0.7, 0.4, 0.3, 0.4, 0.4, 0.4, %fog);
		case 18:
			DaySO.setSun(0.5, 0.3, 0.3, 1);
			DaySO.setSky(0.5, 0.3, 0.3, 0.3, 0.3, 0.3, %fog);
		case 19:
			DaySO.setSun(0.2, 0.2, 0.3, 1);
			DaySO.setSky(0.2, 0.2, 0.3, 0.2, 0.2, 0.2, %fog);
		case 20:
			if(isObject(SunFX)) { SunFX.delete(); }
			DaySO.setSun(0.1, 0.1, 0.3, 1);
			DaySO.setSky(0, 0, 0, 0.1, 0.1, 0.1, %fog);
		case 21:
			DaySO.setSun(0.1, 0.1, 0.2, 1);
			DaySO.setSky(0, 0, 0, 0, 0, 0.1, %fog);
		case 22:
			DaySO.setSun(0.05, 0.05, 0.2, 1);
			DaySO.setSky(0, 0, 0, 0, 0, 0, %fog);
		case 23:
			DaySO.setSun(0.01, 0.01, 0.2, 1);
			DaySO.setSky(0, 0, 0, 0, 0, 0, %fog);
		case 24:
			DaySO.setSun(0, 0, 0.1, 1);
			DaySO.setSky(0, 0, 0, 0, 0, 0, %fog);
	}
}
//if(!isObject(DaySO))
//{
//	new scriptObject(DaySO) { };
//}
