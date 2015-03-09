// --------------------------
// RP Core :: Bricks
// --------------------------

if (isPackage(RP_Core_Bricks))
	deactivatePackage(RP_Core_Bricks);

package RP_Core_Bricks
{
	// Planting a brick
	function fxDTSBrick::onPlant(%brick)
	{
		Parent::onPlant(%brick);
		
		// Loading brick
		if (%brick == $LastLoadedBrick)
			%brick.schedule(1, LoadingBrick);
		else
			%brick.schedule(1, PlantBrick);
	}
	
	// Killing a brick
	function fxDTSBrick::onDeath(%brick)
	{
		%brick.RemoveBrick();
		Parent::onDeath(%brick);
	}
};
RPRegPackage(RP_Core_Bricks);

// When planting a brick
function fxDTSBrick::PlantBrick(%brick)
{
	%brickGroup = getBrickGroupFromObject(%brick);
	%client = %brickGroup.client;
	%datablock = %brick.getDatablock();
	
	// Check admin
	if (!%client.isAdmin && %datablock.adminOnly)
	{
		commandToClient(%client, 'centerPrint', "\c3You must be admin to plant this brick!", 3);
		%brick.schedule(0, "delete");
		return;
	}
}

// When loading a brick
function fxDTSBrick::LoadingBrick(%brick)
{
}

// When removing a brick
function fxDTSBrick::RemoveBrick(%brick)
{
}
