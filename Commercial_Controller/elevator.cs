using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        int ID;
        public string status;
        int amountOfFloors;
        public int currentFloor;
        int screenDisplay;
        Door door;
        public string direction;
        List<int> floorRequestList;
        bool overweight;

        public Elevator(int _id, string _status, int _amountOfFloors, int _currentFloor)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.currentFloor = _currentFloor;
            this.door = new Door(_id, "closed");
            this.floorRequestList = new List<int>();
            this.direction = null;
            this.overweight = false;
            
        }
        public void move()
        {
            while (this.floorRequestList.Count != 0)
            {
                int destination = this.floorRequestList[0];
                this.status = "moving";
                if(this.currentFloor < destination)
                {
                    this.direction = "up";
                    this.sortFloorList();
                    while(this.currentFloor < destination)
                    {
                        this.currentFloor++;
                        this.screenDisplay = this.currentFloor;
                    }
                } else if(this.currentFloor > destination){
                   direction = "down";
                   this.sortFloorList(); 
                   while(this.currentFloor < destination)
                    {
                        this.currentFloor--;
                        this.screenDisplay = this.currentFloor;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.floorRequestList.Remove(0);
            }
            this.status = "idle";
        }
        
        public void sortFloorList()
        {
            if(this.direction == "up")
            {
                this.floorRequestList.Sort();
            } else {
                this.floorRequestList.Sort();
                this.floorRequestList.Reverse();
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
            if(!this.floorRequestList.Contains(requestedFloor))
            {
                this.floorRequestList.Add(requestedFloor);
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