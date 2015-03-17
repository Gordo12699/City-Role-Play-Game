// --------------------------
// RP Jobs :: Game
// --------------------------

if (isPackage(RP_Jobs_Game_Core))
	deactivatePackage(RP_Jobs_Game_Core);

package RP_Jobs_Game_Core
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		RPDB.addValue("jobID", 1); // Tourist
		RPDB.addValue("LaborExp", 0);
		RPDB.addValue("CrimeExp", 0);
		RPDB.addValue("LawExp", 0);
		RPDB.addValue("BusinessExp", 0);
		RPDB.addValue("MedicalExp", 0);
		RPDB.addValue("PoliticalExp", 0);
		RPDB.addValue("shopLicense", 0);
		RPDB.saveDefs();
		
		RP_AddStat("Job", "jobID", 1, "RP::job::name");
		//RP_AddStat("Outfit", "outfit", 0);
		RP_AddStat("Gender", "gender", 0);
		RP_AddStat("Clothed", "clothed", 0);
	}
};
activatePackage(RP_Jobs_Game_Core);

if (isPackage(RP_Jobs_Game))
	deactivatePackage(RP_Jobs_Game);

package RP_Jobs_Game
{
	// RP Player tick
	function CheckTickID(%ID)
	{
		%name = RPDB.get(%ID, "name");
		%client = findClientByName(%name);
		if (!isObject(%client))
			return false;
		
		if (RPModExist("Money"))
		{
			// Get temporary variables
			%job = %client.RPData.value["jobID"];
			%payTime = $RP::job::paytime[%job];
			%pay = $RP::job::pay[%job];
			%times = getWordCount(%payTime);
			
			%payment = 0;
			
			// Sort of payment
			switch (%times)
			{
				// No payment
				case 0:
					%payment = 0;
				// Get paid between two times
				case 2:
					// Get from between
					%from = atoi(getWord(%payTime, 0));
					%to = atoi(getWord(%payTime, 1));
					
					if (%from < $RP::pref::timeHour && %to >= $RP::pref::timeHour)
						%payment = %pay;
					
				// More than 2 or 1
				// Get paid certain times
				default:
					for (%i = 0; %i < %times; %i++)
					{
						if (atoi(getWord(%payTime, %i)) == $RP::pref::timeHour)
						{
							%payment = %pay;
							break;
						}
					}
			}
			
			// Paytime
			if(!%so.valueStudent)
			{
				if (%payment > 0 && %client.getSalary() > 0)
				{
					if (RPModExist("Hunger"))
					{	
						switch(%client.RPData.value["hunger"].valueHunger)
						{
							case 1:
								%penalty = 0;
								// break;
							case 2:
								%penalty = 0.5;
								// break;
							case 3:
								%penalty = 0.8;
								// break;
							case 4:
								%penalty = 0.9;
								// break;
							case 5:
								%penalty = 1;
								// break;
							case 6:
								%penalty = 1;
								// break;
							case 7:
							%penalty = 1;
							// break;
							case 8:
								%penalty = 1.11;
								// break;
							case 9:
								%penalty = 1.25;
								// break;
							case 10:
								%penalty = 1.25;
								// break;
							default:
								%penalty = 0.5;
						}
					}
				
					if(%penalty >= 0.1) //if Hunger mod exists
					{
						%osum = ((%client.getSalary() - %client.brickGroup.taxes) * %penalty);
						%sum = mFloor(%osum);
						// %client.RPData.value["bank"] += %sum;
						// messageClient(%client, '', "\c6 - Your paycheck of \c3$" @ %sum SPC "\c6has been deposited into your account.");
					}
					else
					{
						%sum = %client.getSalary() - %client.brickGroup.taxes;
						%sum = mFloor(%sum);
						// %client.RPData.value["bank"] += %sum;
						// messageClient(%client, '', "\c6 - Your paycheck of \c3$" @ %sum SPC "\c6has been deposited into your account.");
					}
					
					if(%sum > 0)
					{
						if(%sum > $RP::pref::server::PayCheck::Max)
							%sum = $RP::pref::server::PayCheck::Max;
							
						%client.RPData.value["bank"] += %sum;
						messageClient(%client, '', "\c6 - Your paycheck of \c3$" @ %sum SPC "\c6has been deposited into your account.");
					}
					else if((%client.getSalary()) <= 0)
						messageClient(%client, '', "\c6 - You did not receive a paycheck due to your taxes.");
					else if (RPModExist("Hunger"))
					{
						if(%payment <= 0)
							messageClient(%client,'',"\c6 - You did not receive a paycheck because you are starving.");
					}
				}
				
				
				//Bonus Check
				if(%sum * 0.1 >= 1 && getRandom(0, 20) == 20)
				{
					%bonus = mFloor(%sum * getRandom(0.1, 0.25));
					%client.RPData.value["bank"] += %bonus;
					messageClient(%client, '', '\c6 - You also recieved a \c3$%1\c6 bonus along with your paycheck!', %bonus);
				}
			}
			else
			{
				%client.RPData.value["student"]--;
			
				if(!%client.RPData.value["student"])
				{
					%client.RPData.value["education"]++;
					messageClient(%client, '', '\c6 - \c2You graduated\c6, receiving a level \c3%1\c6 diploma!', %client.RPData.value["education"]);
				}
				else
					messageClient(%client, '', '\c6 - Have only \c3%1\c6 days left until you graduate.', %client.RPData.value["student"]);
			}
		}
		
		// Promotion
		Promotion(%client);
		
		Parent::CheckTickID(%ID);
	}
};
RPRegPackage(RP_Jobs_Game);

// Find spawnpoint
function FindSpawn(%search, %id)
{
	%amount = getWordCount($RP::spawnPoints);
	for (%i = 0; %i < %amount; %i++)
	{
		%brick = getWord($RP::spawnPoints, %i);
		
		//Spawn does not exist
		if (!isObject(%brick))
		{
			RemoveSpawn(%brick);
			continue;
		}
		
		// Wrong spawn
		if (strLwr(%brick.getDatablock().spawnData) !$= strLwr(%search @ (%id ? " " @ %id : "")) && strLwr(%search) !$= strLwr("personalSpawn"))
			continue;
		
		// Personal spawn?
		if (strLwr(%search) $= strLwr("personalSpawn") && (strLwr(%brick.getDatablock().spawnData) !$= strLwr("personalSpawn") || getBrickGroupFromObject(%brick).client != %id))
			continue;
		
		%pSpawns = (%pSpawns $= "") ? %brick : %pSpawns SPC %brick;
	}
	
	if (%pSpawns !$= "")
	{
		%spawnBrick = getWord(%pSpawns, getRandom(0, getWordCount(%pSpawns) - 1));
		return vectorSub(%spawnBrick.getWorldBoxCenter(), "0 0" SPC (%spawnBrick.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%spawnBrick.getTransform(), 3, 6);
	}
	
	return "";
}

function gameConnection::getSalary(%client)
{
	return mFloor($RP::job::pay[%client.RPData.value["jobID"]] * (((%client.RPData.value["education"] - $RP::job::education[%client.RPData.value["jobID"]]) / 8) + 1));
}
