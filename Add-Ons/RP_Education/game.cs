// --------------------------
// RP Education :: Game
// --------------------------

if (isPackage(RP_Education_Game_Boot))
	deactivatePackage(RP_Education_Game_Boot);

package RP_Education_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
			
		RPDB.addValue("education", "0");
		RPDB.addValue("student", "0");
		RPDB.saveDefs();

		RP_AddStat("Education", "education", 0);
		RP_AddStat("Student", "student", 0);
		
	}
};
activatePackage(RP_Education_Game_Boot);

package RP_Education_Game
{
	function gameConnection::setScore(%client, %score)
	{
		if($Score::Type $= "Money" || $Score::Type $= "")
			%score = %client.RPData.value["money"] + %client.RPData.value["bank"];
		else if($Score::Type $= "Edu")
			%score = %client.RPData.value["education"];
		parent::setScore(%client, %score);
	}
};
RPRegPackage(RP_Education_Game);
