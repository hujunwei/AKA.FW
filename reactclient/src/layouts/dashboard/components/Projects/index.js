/**
=========================================================
* Material Dashboard 2 React - v2.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

import { useEffect, useState } from "react";

// @mui material components
import Card from "@mui/material/Card";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDBadge from "components/MDBadge";
import MDSpinner from "react-md-spinner";

// Material Dashboard 2 React examples
import DataTable from "examples/Tables/DataTable";

import Constants from "utilities/Constants";
import useToken from "utilities/UseToken";
import useErrorHandler from "utilities/useErrorHandler";

function Projects() {
  const columns = [
    { Header: "name", accessor: "name", width: "40%", align: "left" },
    { Header: "alias", accessor: "alias", width: "20%", align: "left" },
    { Header: "target url", accessor: "targeturl", width: "20%", align: "left" },
    { Header: "status", accessor: "isactive", width: "20%", align: "center" },
  ];
  const { token } = useToken();
  const [loading, setLoading] = useState(false);
  const [loadOfficialUrlsError, setLoadOfficialUrlsError] = useState(false);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [tableData, setTableData] = useState([]);

  async function loadOfficialUrls() {
    setLoadOfficialUrlsError(false);

    return fetch(Constants.API_URL_LIST_OFFCIAL_MAPPINGS, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        setLoading(false);

        if (responseJson.isError) {
          setLoadOfficialUrlsError(true);
          return;
        }

        setLoadOfficialUrlsError(false);
        setTableData(responseJson);
      });
  }

  useEffect(async () => {
    setLoading(true);
    await loadOfficialUrls();
  }, []);

  const rows = tableData.map((url) => ({
    name: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {url.name}
      </MDTypography>
    ),
    alias: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {url.sourceAlias}
      </MDTypography>
    ),
    targeturl: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {url.targetUrl}
      </MDTypography>
    ),
    isactive: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {url.isActive ? (
          <MDBadge color="success" badgeContent="Active" container />
        ) : (
          <MDBadge color="error" badgeContent="Deactivated" container />
        )}
      </MDTypography>
    ),
  }));

  return (
    <Card>
      <MDBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
        <MDBox>
          <MDTypography variant="h6" gutterBottom>
            Official alias links @ FREEWHEEL
          </MDTypography>
        </MDBox>
        <MDBox color="text" px={2}>
          {loading && <MDSpinner aria-label="Loading..." />}
          {loadOfficialUrlsError && renderAlert()}
        </MDBox>
      </MDBox> 
      <MDBox display="flex" alignItems="center">
        <DataTable
          table={{ columns, rows }}
          showTotalEntries={false}
          isSorted={false}
          noEndBorder
          entriesPerPage={false}
        />
      </MDBox>
    </Card>
  );
}

export default Projects;
