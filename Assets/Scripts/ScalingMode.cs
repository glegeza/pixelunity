namespace Assets.Scripts
{
    public enum ScalingMode
    {
        FixedScale, // Use as much screen real estate as possible while maintaining a fixed scale
        FixedPlayArea, // Render at a set resolution and scale up to the desired level
        BestFit, // Render at a set resolution and scale up as far as possible
    }
}