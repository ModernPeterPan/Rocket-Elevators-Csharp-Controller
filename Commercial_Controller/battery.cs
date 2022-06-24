using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {
        private int columnID = 1;
        public static int floorRequestButtonID = 1;
        public static int floor;
        public int ID;
        public string status;
        public List<Column> columnsList;
        List<FloorRequestButton> floorRequestButtonsList;
        List<int> servedFloors;

        public Battery(int _id, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _id;
            this.status = "online";
            this.columnsList = new List<Column>();
            this.floorRequestButtonsList = new List<FloorRequestButton>();

            if (_amountOfBasements > 0)
            {
            createBasementFloorRequestButtons(_amountOfBasements);
            createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
            _amountOfColumns--;
            }

            createFloorRequestButtons(_amountOfFloors);
            createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);
        }

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>();
            int floor = -1;
            for(int i = 0; i < _amountOfBasements; i++)
            {
                servedFloors.Add(floor);
                floor--;
            }

            Column column = new Column(this.columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            this.columnID--;
        }

        public void createColumns (int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn)
        {
            int amountOfFloorsPerColumn = (int)Math.Round((Double)(_amountOfFloors / _amountOfColumns));
            floor = 1;

            for(int i = 0; i < _amountOfColumns; i++)
            {
                List<int> servedFloors = new List<int>();
                
                for(int y = 0; y < amountOfFloorsPerColumn; y++)
                {
                    if(floor <= _amountOfFloors)
                    {
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                //Double check here
                Column column = new Column(this.columnID, "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false);
                columnsList.Add(column);
                this.columnID++;
            }
        }

        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for(int i = 0; i < _amountOfFloors; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Up");
                floorRequestButtonsList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for(int i = 0; i >= _amountOfBasements; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Down");
                floorRequestButtonsList.Add(floorRequestButton);
                buttonFloor--;
                floorRequestButtonID++;
            }
        }

        public Column findBestColumn(int _requestedFloor)
        {
            //Column bestColumn = null;
            foreach (Column column in this.columnsList)
            {
                if (column.servedFloorsList.Contains(_requestedFloor))
                {
                    Column bestColumn = column;
                    return bestColumn;
                }
            }
            return null;
        }

        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column chosenColumn = this.findBestColumn(_requestedFloor);
            Elevator chosenElevator = chosenColumn.findElevator(1, _direction);
            chosenElevator.addNewRequest(1);
            chosenElevator.move();

            chosenElevator.addNewRequest(_requestedFloor);
            chosenElevator.move();
            return(chosenColumn,chosenElevator);
        }
    }
}
