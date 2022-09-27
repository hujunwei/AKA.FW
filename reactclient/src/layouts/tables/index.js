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
import Grid from "@mui/material/Grid";
import Card from "@mui/material/Card";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { IconButton, Divider } from "@mui/material";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDBadge from "components/MDBadge";
import MDButton from "components/MDButton";
import MDSpinner from "react-md-spinner";

// Material Dashboard 2 React example components
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DashboardNavbar from "examples/Navbars/DashboardNavbar";
import Footer from "examples/Footer";
import DataTable from "examples/Tables/DataTable";
import { Navigate } from "react-router-dom";

import Constants from "utilities/Constants";
import useToken from "utilities/UseToken";
import useErrorHandler from "utilities/useErrorHandler";

const formatDate = (dateString) => {
  const options = { year: "numeric", month: "long", day: "numeric" };
  return new Date(dateString).toLocaleDateString(undefined, options);
};

function Tables() {
  const columns = [
    { Header: "name", accessor: "name", width: "20%", align: "left" },
    { Header: "alias", accessor: "alias", width: "20%", align: "left" },
    { Header: "target url", accessor: "targeturl", width: "20%", align: "left" },
    { Header: "updated on", accessor: "updatedat", width: "20%", align: "left" },
    { Header: "status", accessor: "isactive", width: "20%", align: "center" },
    { Header: "actions", accessor: "actions", width: "20%", align: "center" },
  ];
  const { token } = useToken();
  const [loading, setLoading] = useState(false);
  const [loadUserUrlsError, setLoadUserUrlsError] = useState(false);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [tableData, setTableData] = useState([]);

  async function loadUserUrls() {
    return fetch(Constants.API_URL_LIST_USER_MAPPINGS, {
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
          setLoadUserUrlsError(true);
          return;
        }

        setLoadUserUrlsError(false);
        setTableData(responseJson);
      });
  }

  useEffect(async () => {
    setLoading(true);
    await loadUserUrls();
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
    updatedat: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {formatDate(url.updatedAt)}
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
    actions: (
      <MDBox sx={{
        display: 'flex',
        alignItems: 'center'
      }}>
        <IconButton aria-label="edit" size="small">
          <EditIcon color="info" />
        </IconButton>
        <Divider orientation="vertical" sx={{
            height: '20px',
          }}/>
        <IconButton aria-label="delete" size="small">
          <DeleteIcon color="error" />
        </IconButton>
      </MDBox>
    ),
  }));

  return !token ? (
    <Navigate replace to="/authentication/sign-in" />
  ) : (
    <DashboardLayout>
      <DashboardNavbar />
      <MDBox pt={6} pb={3}>
        <Grid container spacing={6}>
          <Grid item xs={12}>
            <Card>
              <MDBox
                mx={2}
                mt={-3}
                py={3}
                px={2}
                variant="gradient"
                bgColor="dark"
                borderRadius="lg"
                coloredShadow="dark"
              >
                <MDBox display="flex" justifyContent="space-between" alignItems="center" p={1}>
                  <MDBox>
                    <MDTypography variant="h6" color="white">
                      View, create or modify your own alias links
                    </MDTypography>
                  </MDBox>
                  <MDBox>
                    {loading && <MDSpinner aria-label="Loading..." />}
                    {!loading && (
                      <MDButton variant="gradient" color="success">
                        <AddIcon>Add</AddIcon>&nbsp; Add
                      </MDButton>
                    )}
                  </MDBox>
                </MDBox>
              </MDBox>
              {loadUserUrlsError && renderAlert()}
              <MDBox pt={3}>
                <DataTable
                  table={{ columns, rows }}
                  isSorted={false}
                  entriesPerPage={false}
                  showTotalEntries={false}
                  noEndBorder
                />
              </MDBox>
            </Card>
          </Grid>
        </Grid>
      </MDBox>
      <Footer />
    </DashboardLayout>
  );
}

export default Tables;
