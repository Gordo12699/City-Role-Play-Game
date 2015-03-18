// ============================================================
// Section 5 : Client->Server Commands
// ============================================================

// Section 5.1 : General Commands
function serverCmdhelp(%client, %section, %term)
{	
	if(!isObject(CRPGHelp))
	{
		echo("CRPG :: Creating new Hellen on Request.");
		new scriptObject(CRPGHelp)
		{
			class = Hellen;
		};
	}
	
	if(%section $= "")
	{
		CRPGHelp.displayHelp(%client, "main");
		
		if(%client.isAdmin)
			CRPGHelp.displayHelp(%client, "main_adminline");
	}
	else
	{
		if(%term)
			messageClient(%client, '', "\c6You must specify a term. Usage: \c3/help [section] [term]\c6.");
		else
		{
			%query = %section @ "_" @ %term;
			
			if(%section $= "list")
			{
				if(%term $= "jobs")
				{
					messageClient(%client, '', "\c6Type \c3/job\c6 then one of the jobs below to become that job.");
					
					for(%a = 1; %a <= JobSO.getJobCount(); %a++)
					{
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name SPC "\c6- Inital Investment: \c3" @ JobSO.job[%a].invest SPC "- \c6Pay: \c3" @ JobSO.job[%a].pay SPC "- \c6Required Education: \c3" @ JobSO.job[%a].education);
						messageClient(%client, '', JobSO.job[%a].helpline);
					}
				}
				else if(%term $= "lots")
				{
					messageClient(%client, '', "\c6This is a list of lots you can plant.");
					
					for(%a = 0; %a < datablockGroup.getCount(); %a++)
					{
						%datablock = datablockGroup.getObject(%a);
						
						if(%datablock.CRPGBrickType == 1)
						{
							messageClient(%client, '', "\c3" @ %datablock.uiName SPC "\c6- Size: \c3" @ %datablock.brickSizeX @ "x" @ %datablock.brickSizeY SPC "\c6Cost: \c3" @ %datablock.initialPrice SPC "\c6Tax: \c3" SPC %datablock.taxAmount);
						}
					}
				}
				else if(%term $= "items")
				{
					messageClient(%client, '', "\c6This is a list of item prices.");
					
					for(%a = 1; isObject($CRPG::prices::weapon::name[%a]); %a++)
					{
						messageClient(%client, '', "\c3" @ $CRPG::prices::weapon::name[%a].uiName SPC "\c6- \c3$" @ $CRPG::prices::weapon::price[%a]);
					}
				}
				else if(%term $= "food")
				{
					messageClient(%client, '', "\c6This is a list of all food stuff.");
					for(%a = 12; %a > 0; %a--)
					{
						messageClient(%client, '', '\c3%1\c6 - \c3$%2', $CRPG::food::name[%a], mFloor((%a) * 4));
					}
				}
				else
					messageClient(%client, '', "\c6You did not request a real list.");
			}
			else
			{
				if(!CRPGHelp.displayHelp(%client, %query))
				{
					messageClient(%client, '', "\c6Help doc '\c3" @ %query @ "\c6' does not exist\c6!");
				}
			}
		}
	}
}

package RealEstateCommandPackage
{
	function serverCmdYes(%client)
	{
		if(isObject(%client.player) && isObject(%client.player.serviceOrigin))
		{
			if(mFloor(VectorDist(%client.player.serviceOrigin.getPosition(), %client.player.getPosition())) < 16)
			{
				if(%client.RPData.value["money"] >= %client.player.serviceFee)
				{
					%ownerBL_ID = %client.player.serviceOrigin.getGroup().bl_id;
					switch$(%client.player.serviceType)
					{		
						case "zone":
							%client.sellZone(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceFee);
					}
				}
				else
				{
					messageClient(%client, '', "\c6You cannot afford this service.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6You are too far away from the service to purchase it!");
			}
		}
		else
		{
			messageClient(%client, '', "\c6You have no active tranfers that you may accept!");
		}
	
		%client.player.serviceType = "";
		%client.player.serviceFee = "";
		%client.player.serviceMarkup = "";
		%client.player.serviceItem = "";
		%client.player.serviceSize = "";
		%client.player.serviceOrigin = "";
		
		parent::serverCmdYes(%client);
	}
};
activatePackage(RealEstateCommandPackage);
