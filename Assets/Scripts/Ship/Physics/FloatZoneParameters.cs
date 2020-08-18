public class FloatZoneParameters
{
    public float floatMax;
    public float floatMin;
    public float deadMax;
    public float deadMin;
    public float stabilizingForce;

    public void SetParameters(
        float floatMax,
        float floatMin,
        float deadMax,
        float deadMin,
        float stabilizingForce
    ){
        this.floatMax = floatMax;
        this.floatMin = floatMin;
        this.deadMax = deadMax;
        this.deadMin = deadMin;
        this.stabilizingForce = stabilizingForce;
    }
}
