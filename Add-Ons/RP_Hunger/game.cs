// --------------------------
// RP Hunger :: Game
// --------------------------

if (isPackage(RP_Hunger_Game_Boot))
	deactivatePackage(RP_Money_Game_Boot);

package RP_Hunger_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		if(!isObject($DamageType::Starvation))
			AddDamageType("Starvation", '\c6 - %1 died of starvation.', '\c6 - %1 died of starvation.', 0.5, 1);
			
		RPDB.addValue("hunger", "7");
		RPDB.saveDefs();

		RP_AddStat("Hunger", "hunger", 0);
		
		CRPG_AssembleHungerEvents();
	}
};
activatePackage(RP_Hunger_Game_Boot);

package RP_Hunger_Game
{
	// RP Player tick
	function CheckTickID(%ID)
	{
		%name = RPDB.get(%ID, "name");
		%client = findClientByName(%name);
		if (!isObject(%client))
			return false;
		
		if (RPModExist("Crime"))
		{
			if(%client.RPData.value["jail"] >= 1)
			{
				if(%client.RPData.value["hunger"] > 3)
				{
					%client.RPData.value["hunger"]--;
				}
				else
				{
					%client.RPData.value["hunger"] = 3;
				}
			}
		}
		
		%client.RPData.value["hunger"]--;
		if(%client.RPData.value["hunger"] == 0)
			%client.RPData.value["hunger"] = 1;
				
		if(isObject(%client.player))
			%client.player.setScale("1 1 1");
		
		Parent::CheckTickID(%ID);
	}
};
RPRegPackage(RP_Hunger_Game);
