using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {
        public int elevatorID;
        public int callButtonID;
        public int ID;
        string status;
        int amountOfFloors;
        int amountOfElevators;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public List<int> servedFloorsList;
        public CallButton callButton;
        public Elevator elevator;

        public Column(int _id, string _status,int _amountOfFloors, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.amountOfElevators = _amountOfElevators;
            this.elevatorsList = new List<Elevator>();
            this.callButtonsList = new List<CallButton>();
            this.servedFloorsList = _servedFloors;

            this.createElevators(_amountOfFloors, _amountOfElevators);
            this.createCallButtons(_amountOfFloors, _isBasement);
        }

        public void createCallButtons(int _amountOfFloors, bool _isBasement)
        {
            if(_isBasement)
            {
                int buttonFloor = -1;
                for(int i = 0; i < _amountOfFloors; i++)
                {
                    callButton = new CallButton(callButtonID, "OFF", buttonFloor, "Up");
                    this.callButtonsList.Add(callButton);
                    buttonFloor--;
                    callButtonID++;
                } 
            } else {
                int buttonFloor = 1;
                for(int i = 0; i < _amountOfFloors; i++)
                {
                    callButton = new CallButton(callButtonID, "OFF", buttonFloor, "Down");
                    this.callButtonsList.Add(callButton);
                    buttonFloor++;
                    callButtonID++;
                }

            }
        }

        public void createElevators (int _amountOfFloors, int _amountOfElevators)
        {
            for(int i = 0; i < _amountOfElevators; i++)
            {
                elevator = new Elevator(elevatorID, "idle", _amountOfFloors, 1);
                this.elevatorsList.Add(elevator);
                elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int userPosition, string direction)
        {
            Elevator elevator = this.findElevator(userPosition, direction);
            elevator.addNewRequest(userPosition);
            elevator.move();

            elevator.addNewRequest(1);
            elevator.move();
            return elevator;
        }

        public Elevator findElevator(int requestedFloor, string requestedDirection)
        {
            BestElevatorInformations bestElevatorInformations;
            Elevator bestElevator = null;
            int bestScore = 6;
            int referenceGap = 10000000;
            if(requestedFloor == 1)
            {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    //How to get from another class file??
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
                    if(1 == elevator.currentFloor && elevator.status == "stopped")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is at the lobby and has no requests
                    } else if(1 == elevator.currentFloor && elevator.status == "idle"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is lower than me and is coming up. It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.
                    } else if(1 > elevator.currentFloor && elevator.direction == "up"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
                    } else if(1 < elevator.currentFloor && elevator.direction == "down"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is not at the first floor, but doesn't have any request
                    } else if(elevator.status == "idle"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is not available, but still could take the call if nothing better is found
                    } else {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.bestElevator;
                    bestScore = bestElevatorInformations.bestScore;
                    referenceGap = bestElevatorInformations.referenceGap;
                }
            } else {
                foreach(Elevator elevator in this.elevatorsList){
                    //The elevator is at the same level as me, and is about to depart to the first floor
                    if(requestedFloor == elevator.currentFloor && elevator.status == "stopped" && requestedDirection == elevator.direction)
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
                    } else if(requestedFloor > elevator.currentFloor && elevator.direction == "up" && requestedDirection == "up"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
                    } else if(requestedFloor < elevator.currentFloor && elevator.direction == "down" && requestedDirection == "down"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is idle and has no requests
                    } else if(elevator.status == "idle"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    //The elevator is not available, but still could take the call if nothing better is found
                    } else {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.bestElevator;
                    bestScore = bestElevatorInformations.bestScore;
                    referenceGap = bestElevatorInformations.referenceGap;
                }
            }
            return bestElevator;
        }

        public BestElevatorInformations checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, int bestScore, int referenceGap, Elevator bestElevator, int floor)
        {
            if(scoreToCheck < bestScore)
            {
                bestScore = scoreToCheck;
                bestElevator = newElevator;
                //How to use ABSOLUTE VALUE
                referenceGap = Math.Abs(newElevator.currentFloor - floor);
            } else if (bestScore == scoreToCheck){
                int gap = Math.Abs(newElevator.currentFloor - floor);
                if(referenceGap > gap){
                    bestElevator = newElevator;
                    referenceGap = gap;
                }
            }
            BestElevatorInformations bestElevatorInformations = new BestElevatorInformations(bestElevator, bestScore, referenceGap);
            return bestElevatorInformations;
        }
    }
}