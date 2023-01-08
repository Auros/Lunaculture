using System;
using System.Collections.Generic;
using Lunaculture.Grids.Objects;
using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridObjectController : MonoBehaviour
    {
        private readonly List<GridObject> _gridObjects = new();
        
        public void Register(GridObject gridObject)
        {
            var current = GetObjectAt(gridObject.Cell);
            if (current is not null)
                throw new InvalidOperationException($"Tried to add a grid object at an already existing location, ({gridObject.Cell.X}, {gridObject.Cell.Y})");
            _gridObjects.Add(gridObject);
        }

        public void Unregister(GridObject gridObject)
        {
            _gridObjects.Remove(gridObject);
        }

        public GridObject? GetObjectAt(GridCell cell)
        {
            foreach (var gridObject in _gridObjects)
                if (cell.Id == gridObject.Cell.Id)
                    return gridObject;
            return null;
        }
    }
}