import React, { useState } from "react";
import Constants from "./utilities/Constants";
import PersonCreateForm from "./components/PersonCreateForm";
import PersonUpdateForm from "./components/PersonUpdateForm";

export default function App() {
  const [people, setPeople] = useState([]);
  const [showingCreateNewPersonForm, setShowingCreateNewPersonForm] =
    useState(false);
  const [showingUpdatePersonForm, setShowingUpdatePersonForm] = useState(null);

  function getPeople() {
    const url = Constants.API_URL_GET_ALL_PEOPLE;

    fetch(url, {
      method: "GET",
    })
      .then((response) => response.json())
      .then((peopleFromServer) => {
        console.log(peopleFromServer);
        setPeople(peopleFromServer);
      })
      .catch((error) => {
        console.log(error);
        alert(error);
      });
  }

  function deletePerson(id) {
    const url = `${Constants.API_URL_DELETE_PERSON}/${id}`;

    fetch(url, {
      method: "DELETE",
    })
      .then(() => {
        onPersonDeleted(id);
      })
      .catch((error) => {
        console.log(error);
      });
  }

  return (
    <div className="container">
      <div className="row min-vh-100">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          {!showingCreateNewPersonForm && !showingUpdatePersonForm && (
            <div>
              <div className="mt-5" class="btn-group" role="group">
                <button
                  onClick={getPeople}
                  className="btn btn-dark btn-secondary btn-lg"
                >
                  Get from server
                </button>
                <button
                  onClick={() => setShowingCreateNewPersonForm(true)}
                  className="btn btn-dark btn-secondary btn-lg"
                >
                  Create new person
                </button>
                <button
                  onClick={() => setPeople([])}
                  className="btn btn-dark btn-secondary btn-lg"
                >
                  Empty list
                </button>
              </div>
            </div>
          )}

          {people.length > 0 &&
            !showingCreateNewPersonForm &&
            !showingUpdatePersonForm &&
            renderPersonsTable()}

          {showingCreateNewPersonForm && (
            <PersonCreateForm onPersonCreated={onPersonCreated} />
          )}

          {!!showingUpdatePersonForm && (
            <PersonUpdateForm
              person={showingUpdatePersonForm}
              onPersonUpdated={onPersonUpdated}
            />
          )}
        </div>
      </div>
    </div>
  );

  function renderPersonsTable() {
    return (
      <div className="table-responsive mt-5">
        <table className="table table-bordered border-dark">
          <thead>
            <tr>
              <th scope="col">Person Id</th>
              <th scope="col">Name</th>
              <th scope="col">Email</th>
              <th scope="col">Address</th>
              <th scope="col">Manage</th>
            </tr>
          </thead>
          <tbody>
            {people.map((p) => (
              <tr key={p.id}>
                <th scope="row">{p.id}</th>
                <td>{p.firstName + p.lastName}</td>
                <td>{p.emailAddresses[0]}</td>
                <td>{p.addresses[0]}</td>
                <td>
                  <div class="btn-group" role="group">
                    <button
                      onClick={() => setShowingUpdatePersonForm(p)}
                      className="btn btn-secondary btn-md"
                    >
                      Update
                    </button>
                    <button
                      onClick={() => {
                        if (
                          window.confirm(
                            `Are you sure you want to delete the person nameed "${p.firstName} ${p.lastName}"?`
                          )
                        )
                          deletePerson(p.id);
                      }}
                      className="btn btn-secondary btn-md"
                    >
                      Delete
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }

  function onPersonCreated(createdPerson) {
    setShowingCreateNewPersonForm(false);

    if (!!createdPerson) {
       alert(
         `Person: ${createdPerson.firstName} ${createdPerson.lastName} created`
       );

       getPeople();
    }
  }

  function onPersonUpdated(updatedPerson) {
    setShowingUpdatePersonForm(null);

    if (updatedPerson === null) {
      return;
    }

    let peopleCopy = [...people];

    const index = peopleCopy.findIndex((peopleCopyPerson, currentIndex) => peopleCopyPerson.id === updatedPerson.id);

    if (index !== -1) {
      peopleCopy[index] = updatedPerson;
    }

    setPeople(peopleCopy);

    alert(
      `Person successfully updated with name "${updatedPerson.firstName} ${updatedPerson.lastName}"`
    );
  }

  function onPersonDeleted(deletedPersonId) {
    let peopleCopy = [...people];

    const index = peopleCopy.findIndex(
      (peopleCopyPerson, currentIndex) =>
        peopleCopyPerson.id === deletedPersonId
    );

    if (index !== -1) {
      peopleCopy.splice(index, 1);
    }

    setPeople(peopleCopy);

    alert(
      "Person successfully deleted. After clicking OK, look at the table below to see your person disappear."
    );
  }
}
