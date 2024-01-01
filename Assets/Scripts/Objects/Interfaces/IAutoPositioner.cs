namespace Assets.Scripts.Objects.Interfaces
{
    internal interface IAutoPositioner
    {
        void Position(IGrabable grabableObject);

        void UnPosition(IGrabable grabableObject);

        bool CanPosition(IGrabable grabableObject);

        bool IsPositionOccupiedByThisObject(IGrabable possiblePositionedObject);
    }
}
