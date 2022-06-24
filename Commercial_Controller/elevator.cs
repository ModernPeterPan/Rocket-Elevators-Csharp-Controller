using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int currentFloor;
        public int screenDisplay;
        public Door door;
        public string direction;
        public List<int> floorRequestsList;
        public List<int> completedRequestsList;
        public bool overweight;

        public Elevator(int _id, string _status, int _amountOfFloors, int _currentFloor)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.currentFloor = _currentFloor;
            this.door = new Door(_id, "closed");
            this.floorRequestsList = new List<int>();
            this.completedRequestsList = new List<int>();
            this.direction = null;
            this.overweight = false;
            
        }
        public void move()
        {
            while (this.floorRequestsList.Count != 0)
            {
                //this.sortFloorList();
                int destination = this.floorRequestsList[0];
                this.status = "moving";
                if(this.direction == "up")
                {
                    while(this.currentFloor < destination)
                    {
                        this.currentFloor++;
                    }
                } else if(this.direction == "down"){
                   while(this.currentFloor > destination)
                    {
                        this.currentFloor--;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.floorRequestsList.RemoveAt(0);
                this.completedRequestsList.Add(destination);
            }
            this.status = "idle";
            this.direction = "";
        }
        
        public void sortFloorList()
        {
            if(this.direction == "up")
            {
                this.floorRequestsList.Sort();
            } else {
                this.floorRequestsList.Sort();
                this.floorRequestsList.Reverse();
            }
        }

        public void operateDoors()
        {
            bool obstruction = false;
            this.door.status = "opened";
            //Wait 5 seconds
            if(!this.overweight)
            {
                this.door.status = "closing";
                if (!obstruction)
                {
                    this.door.status = "closed";
                } else {
                    this.operateDoors();
                }
            } else {
                while(overweight)
                {
                    //Activate overweight alarm
                }
                this.operateDoors();
            }
        }

        public void addNewRequest(int requestedFloor)
        {
            if(!this.floorRequestsList.Contains(requestedFloor))
            {
                this.floorRequestsList.Add(requestedFloor);
            }
            if(this.currentFloor < requestedFloor)
            {
                this.direction = "up";
            }   
            if(this.currentFloor > requestedFloor)
            {
                this.direction = "down";
            }         
        }
    }
}