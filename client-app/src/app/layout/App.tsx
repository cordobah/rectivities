    import React, {useEffect, useState} from 'react';
    import axios from "axios";
    import {Container} from 'semantic-ui-react';
    import {Activity} from "../modules/activity";
    import NavBar from "./NavBar";
    import ActivityDashboard from "../../features/Activity/Dashboard/ActivityDashboard";
    import {v4 as uuid} from "uuid";



    function App() {
      const [activities, setActivities] = useState<Activity[]>([]);
      const [selectedActivity, setSelectedActivity ] = useState<Activity|undefined>(undefined);
      const [editMode, setEditMode] = useState(false);

      function handleSelectActivity(id: string) {
          setSelectedActivity(activities.find(a => a.id === id));
      }

      function handleCancelActivity() {
          setSelectedActivity(undefined);
      }
       function handleFormOpen(id?: string) {
          id ? handleSelectActivity(id) : handleCancelActivity();
          setEditMode(true);
       }
        function handleFormClose() {
            setEditMode(false);
        }
        
        function handleCreateOrEditActivity(activity: Activity) {
          activity.id
              ? setActivities([...activities.filter(a => a.id !== activity.id), activity])
              : setActivities([...activities,{...activity, id: uuid()}]);
          setEditMode(false);
          setSelectedActivity(activity);
        }

        function handleDeleteActivity(id: string) {
          setActivities([...activities.filter(a => a.id !== id)]);
          setSelectedActivity(undefined);
        }

        useEffect(() => {
            axios.get<Activity[]>('https://localhost:7000/api/activities')
                .then(response => {
                    setActivities(response.data);
                })
        }, []);


        return (
            <>
                <NavBar openForm={handleFormOpen}/>
                <Container style={{marginTop: '7em'}}>
                    <ActivityDashboard
                        activities={activities}
                        selectedActivity={selectedActivity}
                        selectActivity={handleSelectActivity}
                        cancelSelectActivity={handleCancelActivity}
                        editMode={editMode}
                        openForm={handleFormOpen}
                        closeForm={handleFormClose}
                        createOrEdit={handleCreateOrEditActivity}
                        deleteActivity={handleDeleteActivity}
                    />
                </Container>
            </>
        );
    }

    export default App;