using System;
using UnityEngine;
using DaggerfallConnect;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;

    //this class initializes the mod.
    public class RandomDungeonsLoader : MonoBehaviour
    {
        public static Mod mod;
        public static GameObject go;

        //like in the last example, this is used to setup the Mod.  This gets called at Start state.
        [Invoke]
        public static void InitAtStartState(InitParams initParams)
        {
            mod = initParams.Mod;
            go = new GameObject(mod.Title);
            Debug.Log("Started setup of : " + mod.Title);

            mod.IsReady = true;
        }

        [Invoke(StateManager.StateTypes.Game)]
        public static void InitAtGameState(InitParams initParams)
        {

        }

        void GenerateRandomDungeon(ref DFLocation dfLocation)
        {
            // Must not be called for main story dungeons
            if (DaggerfallWorkshop.DaggerfallDungeon.IsMainStoryDungeon(dfLocation.MapTableData.MapId))
                throw new Exception("GenerateSmallerDungeon() must not be called on a main story dungeon.");

            // Ignore small dungeons under threshold - this will exclude already small crypts and the like
            if (dfLocation.Dungeon.Blocks == null)
                return;

            // Seed random generation with map ID so we get the same layout each time map is looked up
            DaggerfallWorkshop.DFRandom.Seed = (uint)dfLocation.MapTableData.MapId;
            int threshold = DaggerfallWorkshop.DFRandom.random_range_inclusive(5,15);
            //threshold = 7;
            // Ignore small dungeons under threshold - this will exclude already small crypts and the like
            //if (dfLocation.Dungeon.Blocks == null || dfLocation.Dungeon.Blocks.Length <= threshold)
            if (dfLocation.Dungeon.Blocks == null)
                return;

            int mainBlocks = threshold - 4;
            int totalBlocks;
            if(threshold == 5) {
                mainBlocks = 1;
                totalBlocks = 5;
            } else {
                totalBlocks = (mainBlocks * 3) + 2;
            }
            // Generate new dungeon layout with smallest viable dungeon (1x normal block surrounded by 4x border blocks)
            DFLocation.DungeonBlock[] layout = new DFLocation.DungeonBlock[totalBlocks];

            bool startBlock = true;
            sbyte x = 0;
            sbyte z = 0;

            int index = 0;

            for(int i = 0; i < mainBlocks; i++) {
                layout[index] = GenerateRDBBlock(x, z, false, startBlock);
                startBlock = false;
                index++;
                x++;
            }

            if(mainBlocks == 1) {
                layout[1] = GenerateRDBBlock(0, -1, true, false);          // North border block
                layout[2] = GenerateRDBBlock(-1, 0, true, false);          // West border block
                layout[3] = GenerateRDBBlock(1, 0, true, false);           // East border block
                layout[4] = GenerateRDBBlock(0, 1, true, false);           // South border block
            } else {
                sbyte offsetX = 0;
                sbyte offsetZ = 1;
                sbyte offsetZ2 = -1;

                for(int i = 0; i < mainBlocks; i++) {
                    layout[index] = GenerateRDBBlock(offsetX, offsetZ, true, false);
                    index++;
                    layout[index] = GenerateRDBBlock(offsetX, offsetZ2, true, false);
                    index++;
                    offsetX++;
                }

                layout[index] = GenerateRDBBlock(-1, 0, true, false);
                index++;
                x = (sbyte) (mainBlocks);
                layout[index] = GenerateRDBBlock(x, 0, true, false);
            }

            // Inject new block array into location
            dfLocation.Dungeon.Blocks = layout;

        }

        DFLocation.DungeonBlock GenerateRDBBlock(sbyte x, sbyte z, bool borderBlock, bool startingBlock)
        {
            // Get random block from reference location and overwrite some properties
            DFLocation.DungeonBlock block = GetRandomBlock(borderBlock);
            block.X = x;
            block.Z = z;
            block.IsStartingBlock = startingBlock;

            return block;
        }

        /// <summary>
        /// Gets a random block from reference location.
        /// </summary>
        /// <param name="borderBlock">True to select a border block, false to select an interior block.</param>
        /// <param name="dfLocation">Reference location to select a random block from.</param>
        /// <returns>DFLocation.DungeonBlock</returns>
        DFLocation.DungeonBlock GetRandomBlock(bool borderBlock)
        {
            DFLocation.DungeonBlock block = new DFLocation.DungeonBlock();
            block.X = 0;
            block.Z = 0;
            block.IsStartingBlock = false;
            block.WaterLevel = 10000;
            block.CastleBlock = false;
            block.BlockName = GetBlockName(borderBlock);
            return block;
        }

        string GetBlockName(bool borderBlock) {
            string[] borderBlocks = new string[]{
                "B0000000.RDB",
                "B0000001.RDB",
                "B0000002.RDB",
                "B0000003.RDB",
                "B0000004.RDB",
                "B0000005.RDB",
                "B0000006.RDB",
                "B0000007.RDB",
                "B0000008.RDB",
                "B0000009.RDB",
                "B0000010.RDB",
                "B0000011.RDB",
                "B0000012.RDB",
                "B0000013.RDB",
                "B0000014.RDB"
            };

            string[] normalBlocks = new string[]{
                "N0000000.RDB",
                "N0000001.RDB",
                "N0000002.RDB",
                "N0000003.RDB",
                "N0000004.RDB",
                "N0000005.RDB",
                "N0000006.RDB",
                "N0000007.RDB",
                "N0000008.RDB",
                "N0000009.RDB",
                "N0000010.RDB",
                "N0000011.RDB",
                "N0000012.RDB",
                "N0000013.RDB",
                "N0000014.RDB",
                "N0000015.RDB",
                "N0000016.RDB",
                "N0000017.RDB",
                "N0000018.RDB",
                "N0000019.RDB",
                "N0000020.RDB",
                "N0000021.RDB",
                "N0000022.RDB",
                "N0000023.RDB",
                "N0000024.RDB",
                "N0000025.RDB",
                "N0000026.RDB",
                "N0000027.RDB",
                "N0000028.RDB",
                "N0000029.RDB",
                "N0000030.RDB",
                "N0000031.RDB",
                "N0000032.RDB",
                "N0000033.RDB",
                "N0000034.RDB",
                "N0000035.RDB",
                "N0000036.RDB",
                "N0000037.RDB",
                "N0000038.RDB",
                "N0000039.RDB",
                "N0000040.RDB",
                "N0000041.RDB",
                "N0000042.RDB",
                "N0000043.RDB",
                "N0000044.RDB",
                "N0000045.RDB",
                "N0000046.RDB",
                "N0000047.RDB",
                "N0000048.RDB",
                "N0000049.RDB",
                "N0000050.RDB",
                "N0000051.RDB",
                "N0000052.RDB",
                "N0000053.RDB",
                "N0000054.RDB",
                "N0000055.RDB",
                "N0000056.RDB",
                "N0000057.RDB",
                "N0000058.RDB",
                "N0000059.RDB",
                "N0000060.RDB",
                "N0000061.RDB",
                "N0000062.RDB",
                "N0000063.RDB",
                "N0000064.RDB",
                "N0000065.RDB",
                "N0000066.RDB",
                "N0000067.RDB",
                "N0000068.RDB",
                "N0000069.RDB",
                "N0000070.RDB",
                "N0000071.RDB",
                "N0000072.RDB",
                "N0000073.RDB",
                "N0000074.RDB",
                "N0000075.RDB",
                "N0000076.RDB",
                "N0000077.RDB",
                "N0000078.RDB",
                "N0000079.RDB",
                "N0000080.RDB",
                "N0000081.RDB",
                "N0000082.RDB",
                "N0000083.RDB",
                "N0000084.RDB",
                "N0000085.RDB",
                "N0000086.RDB",
                "N0000087.RDB",
                "N0000088.RDB",
                "N0000089.RDB",
                "N0000090.RDB",
                "N0000091.RDB",
                "N0000092.RDB"
            };

            string[] wetBlocks = new string[]{
                "W0000000.RDB",
                "W0000001.RDB",
                "W0000002.RDB",
                "W0000003.RDB",
                "W0000004.RDB",
                "W0000005.RDB",
                "W0000006.RDB",
                "W0000007.RDB",
                "W0000008.RDB",
                "W0000009.RDB",
                "W0000010.RDB",
                "W0000011.RDB",
                "W0000012.RDB",
                "W0000013.RDB",
                "W0000014.RDB",
                "W0000015.RDB",
                "W0000016.RDB",
                "W0000017.RDB",
                "W0000018.RDB",
                "W0000019.RDB",
                "W0000020.RDB",
                "W0000021.RDB",
                "W0000022.RDB",
                "W0000023.RDB",
                "W0000024.RDB",
                "W0000025.RDB",
                "W0000026.RDB",
                "W0000027.RDB",
                "W0000028.RDB",
                "W0000029.RDB"
            };

            if(borderBlock) {
                return borderBlocks[DaggerfallWorkshop.DFRandom.random_range_inclusive(0, borderBlocks.Length - 1)];
            }
            return normalBlocks[DaggerfallWorkshop.DFRandom.random_range_inclusive(0, normalBlocks.Length - 1)];
        }
    }