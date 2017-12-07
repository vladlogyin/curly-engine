using System;
namespace CurlyEngine.Server
{
    public class Block
    {
        /// <summary>
        /// Block id used in-game
        /// </summary>
        public int ID;
        /// <summary>
        /// Name of block used for display
        /// </summary>
        public string Name;
        /// <summary>
        /// Descriprion of the block
        /// </summary>
        public string Description;
        /// <summary>
        /// Type of the block
        /// </summary>
        public BlockType type;
        /// <summary>
        /// List of all blocks
        /// </summary>
        public static Block[] Blocks =
        {
            new Block(0,BlockType.Transparent,"Air","Test Description"),
            new Block(1,BlockType.Opaque,"Rock","Test Description"),
            new Block(2,BlockType.Opaque,"Dirt","Test Description"),
            new Block(3,BlockType.Liquid,"Water","Test Description"),
        };
        public Block(int id,BlockType typ, string name="Unknown Item",string description="")
        {
            this.type = typ;
            this.ID = id;
            this.Name = name;
            this.Description = description;
        }
    }
    /// <summary>
    /// Enumaration of all block types
    /// </summary>
    public enum BlockType : int
    {
        /// <summary>
        /// Blocktype liquid which marks a block that slows down movement and disables certain items
        /// </summary>
        Liquid = -2,
        /// <summary>
        /// Anything gasious like air
        /// </summary>
        Transparent = -1,
        /// <summary>
        /// Walkthrough but has a render texture
        /// </summary>
        Walkthrough = 1,
        /// <summary>
        /// Any usual solid block
        /// </summary>
        Opaque = 0,
        /// <summary>
        /// Walkthrough, tells game to look for entity id
        /// </summary>
        Entity = 2
    };
}