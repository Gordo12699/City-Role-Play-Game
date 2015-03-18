// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// Project				:	RP_Weather
// Author				:	JJstorm
// Description			        :	Weather For RP_Core
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

// Weather

datablock PrecipitationData(RP_Weather_Rain)
{
   dropTexture = "Add-Ons/RP_Weather/textures/droplet";
   splashTexture = "Add-Ons/RP_Weather/textures/splash";
   dropSize = 0.50;
   splashSize = 0.2;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock decalData(RP_Weather_Rain_Texture)
{
	textureName = "Add-Ons/RP_Weather/textures/droplet";
	preload = true;
};

datablock decalData(RP_Weather_Rain_Texture)
{
	textureName = "Add-Ons/RP_Weather/textures/sunflare";
	preload = true;
};


datablock decalData(RP_Weather_Splash_Texture)
{
	textureName = "Add-Ons/RP_Weather/textures/splash";
	preload = true;
};

// Sounds

datablock AudioDescription(WeatherSound)
{
	volume = 0.9;
	isLooping = false;
	is3D = false;
	type = $DefaultAudioType;
};

datablock AudioProfile(RainSound)
{
	filename = "Add-Ons/RP_Weather/sounds/RainSound.wav";
	description = "WeatherSound";
	preload = true;
};

datablock AudioProfile(SnowSound)
{
	filename = "Add-Ons/RP_Weather/sounds/SnowSound.wav";
	description = "WeatherSound";
	preload = true;
};
