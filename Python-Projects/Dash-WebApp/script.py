"""
App visant à étudier les données de demandes de valeurs foncières
selon différents critères (département, secteur de Paris, région)
"""
import dash
import pandas as pd
import plotly.express as px
import dash_bootstrap_components as dbc
from dash import Input, Output, dcc, html

DEPARTEMENT = ["39", "77", "93", "95"]
SECTEURS = ["NE", "NO", "SO", "SE"]
REGIONS = ["ARA", "Bretagne", "HDF", "IDF", "Normandie", "PDL"]

df_dpt = {}
for dpt in DEPARTEMENT:
    source_dpt = f"Data/Prix_Appart_par_ville/Prix_Appart_par_ville_{dpt}_2021.txt"
    df_dpt[dpt] = pd.read_csv(source_dpt, sep=";", header=0)

df_sect = {}
for sect in SECTEURS:
    source_sect = f"Data/Paris/Prix_Appartement_Paris_{sect}_2021.txt"
    df_sect[sect] = pd.read_csv(source_sect, sep=";", header=0)

df_region = {}
for region in REGIONS:
    source_region = f"Data/Evolution_prix_Region_2021/Evolution_prix_{region}_2021.txt"
    df_region[region] = pd.read_csv(source_region, sep=";", header=0)
    df_region[region]["Date mutation"] = pd.to_datetime(
        df_region[region]["Date mutation"]
    )


app = dash.Dash(
    name="Demandes de valeurs foncières", external_stylesheets=[dbc.themes.BOOTSTRAP]
)


menu = dbc.Container(
    id="menu",
    children=[
        html.H1("Demandes de valeurs foncières", style={"margin-top": "10px"}),
        html.Hr(),
        dbc.Tabs(
            [
                dbc.Tab(label="Accueil", tab_id="home"),
                dbc.Tab(label="Communes", tab_id="communes"),
                dbc.Tab(label="Paris", tab_id="paris"),
                dbc.Tab(label="Régions", tab_id="regions"),
            ],
            id="tabs",
            active_tab="home",
            style={},
        ),
    ],
)

content = dbc.Container(
    id="page-content",
    children=[
        html.Div(
             id="parameters-title", style={"margin-top": "10px"}
        ),
        dcc.Dropdown(id="parameters", style={"display": "none"}),
    ],
)

graph = dbc.Container(id="tab-content")

app.layout = html.Div(
    [
        menu,
        content,
        graph,
    ]
)


@app.callback(
    Output("parameters-title", "children"),
    Output("parameters", "style"),
    Output("parameters", "options"),
    Output("parameters", "value"),
    [Input("tabs", "active_tab")],
)
def render_page_content(active_tab):
    if active_tab == "communes":
        return (
            html.H2(className="display-6","Département"),
            {"display": "block"},
            [{"label": dep, "value": dep} for dep in DEPARTEMENT],
            DEPARTEMENT[0],
        )
    if active_tab == "paris":
        return (
            html.H2(className="display-6","Secteur"),
            {"display": "block"},
            [{"label": sct, "value": sct} for sct in SECTEURS],
            SECTEURS[0],
        )
    if active_tab == "regions":
        return (
            html.H2(className="display-6","Secteur"),
            {"display": "block"},
            [{"label": reg, "value": reg} for reg in REGIONS],
            REGIONS[0],
        )
    return (html.P(("App visant à étudier les données de demandes de valeurs foncières"+
                    "selon différents critères (département, secteur de Paris, région)")),
                    {"display": "none"}, [""], "")


@app.callback(Output("tab-content", "children"), [Input("parameters", "value")])
def generate_graph(value):
    if value != "":
        if value in DEPARTEMENT:
            titre1 = f"Prix au m2 par commune du département {value} en 2021"
            fig = px.line(
                df_dpt[value], x="Commune", y="Prix au m2 (Surface bati)", title=titre1
            )
            return dcc.Graph(figure=fig)
        if value in SECTEURS:
            titre2 = ("Prix du m2 en fonction de la surface "
                      + f"par nombre de pièce dans le {value} de Paris")
            fig2 = px.scatter(
                df_sect[value],
                x="Surface reelle bati",
                y="Prix au m2 (Surface bati)",
                title=titre2,
                color="Quartier",
                facet_col="Nombre pieces principales",
            )
            return dcc.Graph(figure=fig2)
        if value in REGIONS:
            titre3 = f"Evolution du prix pour la région {value} en 2021"
            fig3 = px.scatter(
                df_region[value],
                x="Date mutation",
                y="Prix au m2 (Surface bati)",
                title=titre3,
                trendline="rolling",
                trendline_options= {"function": 'median', "window": 7},
                trendline_color_override="red",
            )
            return dcc.Graph(figure=fig3)
    return html.P("")


if __name__ == "__main__":
    app.run_server(debug=True, port=8888)
