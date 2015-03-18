// --------------------------
// RP Resources :: Game
// --------------------------

if (isPackage(RP_Resources_Game))
	deactivatePackage(RP_Resources_Game);

package RP_Resources_Game
{
	// Booting game
	function BootRP()
	{
		if (!Parent::BootRP())
			return false;
		
		RPDB.addvalue("ore", 0);
		RPDB.addvalue("wood", 0);
		RPDB.addvalue("fish", 0);
		RPDB.saveDefs();
		
		RP_AddStat($RP::pref::oreName, "ore", 1, "", "", "", 1);
		RP_AddStat($RP::pref::woodName, "wood", 1, "", "", "", 1);
		RP_AddStat($RP::pref::fishName, "fish", 1, "", "", "", 1);
		
		return true;
	}
};
RPRegPackage(RP_Resources_Game);
