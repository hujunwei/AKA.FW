import React, { useState } from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";

import Constants from "./utilities/Constants";
import PersonCreateForm from "./components/PersonCreateForm";
import PersonUpdateForm from "./components/PersonUpdateForm";
import Dashboard from "./components/Dashboard";
import Preferences from "./components/Preference";
import Login from "./components/Login";

import useToken from "./utilities/UseToken";

export default function App() {
  const { token, setToken } = useToken();

  if (!token) {
    return <Login setToken={setToken} />;
  }

  return (
    <div className="wrapper">
      <h1>Application</h1>
      <BrowserRouter>
        <Switch>
          <Route path="/dashboard">
            <Dashboard />
          </Route>
          <Route path="/preferences">
            <Preferences />
          </Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
}


// export default function App() {
//   const [people, setPeople] = useState([]);
//   const [showingCreateNewPersonForm, setShowingCreateNewPersonForm] =
//     useState(false);
//   const [showingUpdatePersonForm, setShowingUpdatePersonForm] = useState(null);

//   function getPeople() {
//     const url = Constants.API_URL_GET_ALL_PEOPLE;

//     fetch(url, {
//       method: "GET",
//     })
//       .then((response) => response.json())
//       .then((peopleFromServer) => {
//         console.log(peopleFromServer);
//         setPeople(peopleFromServer);
//       })
//       .catch((error) => {
//         console.log(error);
//         alert(error);
//       });
//   }

//   function deletePerson(id) {
//     const url = `${Constants.API_URL_DELETE_PERSON}/${id}`;

//     fetch(url, {
//       method: "DELETE",
//     })
//       .then(() => {
//         onPersonDeleted(id);
//       })
//       .catch((error) => {
//         console.log(error);
//       });
//   }

//   return (
//     <div>
//       {!showingCreateNewPersonForm && !showingUpdatePersonForm && (
//         <header class="p-3 text-bg-dark">
//           <div class="container">
//             <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
//               <a
//                 href="/"
//                 class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none"
//               >
//                 <div
//                   class="bi me-4"
//                   width="40"
//                   height="40"
//                   role="img"
//                   aria-label="Bootstrap"
//                 >
//                 <img src={require("./assets/fwlogo.png")} alt="FW" />
//                 </div>
//               </a>

//               <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
//                 <li>
//                   <a
//                     href="javascript:void(0);"
//                     onClick={getPeople}
//                     class="nav-link px-2 text-white"
//                   >
//                     Get from server
//                   </a>
//                 </li>
//                 <li>
//                   <a
//                     href="javascript:void(0);"
//                     onClick={() => setShowingCreateNewPersonForm(true)}
//                     class="nav-link px-2 text-white"
//                   >
//                     Create new person
//                   </a>
//                 </li>
//                 <li>
//                   <a
//                     href="javascript:void(0);"
//                     onClick={() => setPeople([])}
//                     class="nav-link px-2 text-white"
//                   >
//                     Empty list
//                   </a>
//                 </li>
//                 <li>
//                   <a href="/" class="nav-link px-2 text-white">
//                     FAQs
//                   </a>
//                 </li>
//                 <li>
//                   <a href="/" class="nav-link px-2 text-white">
//                     About
//                   </a>
//                 </li>
//               </ul>

//               <form
//                 class="col-12 col-lg-auto mb-3 mb-lg-0 me-lg-3"
//                 role="search"
//               >
//                 <input
//                   type="search"
//                   class="form-control form-control-dark text-bg-dark"
//                   placeholder="Search..."
//                   aria-label="Search"
//                 />
//               </form>

//               <div class="text-end">
//                 <button type="button" class="btn btn-outline-light me-2">
//                   Login
//                 </button>
//                 <button type="button" class="btn btn-warning">
//                   Sign-up
//                 </button>
//               </div>
//             </div>
//           </div>
//         </header>
//       )}

//       {people.length > 0 &&
//         !showingCreateNewPersonForm &&
//         !showingUpdatePersonForm &&
//         renderPersonsTable()}

//       {showingCreateNewPersonForm && (
//         <PersonCreateForm onPersonCreated={onPersonCreated} />
//       )}

//       {!!showingUpdatePersonForm && (
//         <PersonUpdateForm
//           person={showingUpdatePersonForm}
//           onPersonUpdated={onPersonUpdated}
//         />
//       )}
//     </div>
//   );

//   function renderPersonsTable() {
//     return (
//       <div className="table-responsive mt-5">
//         <table className="table table-bordered border-dark">
//           <thead>
//             <tr>
//               <th scope="col">Person Id</th>
//               <th scope="col">Name</th>
//               <th scope="col">Email</th>
//               <th scope="col">Address</th>
//               <th scope="col">Manage</th>
//             </tr>
//           </thead>
//           <tbody>
//             {people.map((p) => (
//               <tr key={p.id}>
//                 <th scope="row">{p.id}</th>
//                 <td>{p.firstName + p.lastName}</td>
//                 <td>{p.emailAddresses[0]}</td>
//                 <td>{p.addresses[0]}</td>
//                 <td>
//                   <div class="btn-group" role="group">
//                     <button
//                       onClick={() => setShowingUpdatePersonForm(p)}
//                       className="btn btn-secondary btn-md"
//                     >
//                       Update
//                     </button>
//                     <button
//                       onClick={() => {
//                         if (
//                           window.confirm(
//                             `Are you sure you want to delete the person nameed "${p.firstName} ${p.lastName}"?`
//                           )
//                         )
//                           deletePerson(p.id);
//                       }}
//                       className="btn btn-secondary btn-md"
//                     >
//                       Delete
//                     </button>
//                   </div>
//                 </td>
//               </tr>
//             ))}
//           </tbody>
//         </table>
//       </div>
//     );
//   }

//   function onPersonCreated(createdPerson) {
//     setShowingCreateNewPersonForm(false);

//     if (!!createdPerson) {
//       alert(
//         `Person: ${createdPerson.firstName} ${createdPerson.lastName} created`
//       );

//       getPeople();
//     }
//   }

//   function onPersonUpdated(updatedPerson) {
//     setShowingUpdatePersonForm(null);

//     if (updatedPerson === null) {
//       return;
//     }

//     let peopleCopy = [...people];

//     const index = peopleCopy.findIndex(
//       (peopleCopyPerson, currentIndex) =>
//         peopleCopyPerson.id === updatedPerson.id
//     );

//     if (index !== -1) {
//       peopleCopy[index] = updatedPerson;
//     }

//     setPeople(peopleCopy);

//     alert(
//       `Person successfully updated with name "${updatedPerson.firstName} ${updatedPerson.lastName}"`
//     );
//   }

//   function onPersonDeleted(deletedPersonId) {
//     let peopleCopy = [...people];

//     const index = peopleCopy.findIndex(
//       (peopleCopyPerson, currentIndex) =>
//         peopleCopyPerson.id === deletedPersonId
//     );

//     if (index !== -1) {
//       peopleCopy.splice(index, 1);
//     }

//     setPeople(peopleCopy);

//     alert(
//       "Person successfully deleted. After clicking OK, look at the table below to see your person disappear."
//     );
//   }
// }
