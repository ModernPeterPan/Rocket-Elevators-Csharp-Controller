using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {
        static int columnID;
        static int floorRequestButtonID = 1;
        static int floor;
        int ID;
        string status;
        List<int> columnsList;
        List<int> floorRequestsButtonsList;
        List<int> servedFloors;

        public Battery(int _id, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _id;
            this.status = "online";
            this.columnsList = new List<int>();
            this.floorRequestsButtonsList = new List<int>();

            if (_amountOfBasements > 0)
            {
            this.createBasementFloorRequestButtons(_amountOfBasements);
            this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
            _amountOfColumns--;
            }

            this.createFloorRequestButtons(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);
        }

        static void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>();
            int floor = -1;
            for(int i = 0; i < _amountOfBasements; i++)
            {
                servedFloors.Add(floor);
                floor--;
            }

            Column column = new Column(columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            this.columnsList.Add(column);
            columnID--;
        }

        static void createColumns (int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            Double amountOfFloorsPerColumn = Math.Round((Double)(_amountOfFloors / _amountOfColumns));
            floor = 1;

            for(int i = 1; i < _amountOfColumns; i++)
            {
                List<int> servedFloors = new List<int>();
                for(int i = 1; i < amountOfFloorsPerColumn; i++)
                {
                    if(floor <= _amountOfFloors)
                    {
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
            
                column = new Column(columnID, "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false)
                columnsList.Add(column);
                columnID++;
            }
        }

        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for(int i = 1; i < _amountOfFloors; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Up");
                this.floorButtonsList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }

        static void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for(int i = 0; i >= _amountOfBasements; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Down");
                floorButtonsList.Add(floorRequestButton);
                buttonFloor--;
                floorRequestButtonID++;
            }
        }

        public Column findBestColumn(int _requestedFloor)
        {
            foreach(int column in this.columnsList)
            {
                //IF column servedFloorsList CONTAINS _requestedFloor
                if(column.servedFloorsList(_requestedFloor))
                {
                    return column;
                }
            }
            return new Column();
        }

        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column column = this.findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(_requestedFloor);
            elevator.move();
        }
    }
}
