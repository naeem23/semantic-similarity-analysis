import pandas as pd
import plotly.express as px

# read csv file
df = pd.read_csv('similarity_output.csv', skipinitialspace=True)

# Drop unnamed columns (usually with column names like 'Unnamed: x')
df = df.loc[:, ~df.columns.str.contains('^Unnamed')]

# 2D scatter ploting using plotly express 
# Convert the wide format to long format using melt()
"""df_long = df.melt(id_vars=["Source Content"], var_name="Reference", value_name="Similarity")
fig = px.scatter(
        df_long, 
        x='Reference', 
        y='Similarity', 
        color="Source Content",
        labels={'Reference': 'Reference', 'Similarity': 'Similarity Score'},
        title="2D Similarity Score Plot"
    )

# Fixing y-axis range for 2d scatter 
fig.update_layout(yaxis=dict(range=[-1, 1]))"""


# 3D scatter Ploting using plotly express
# Store original names for hover info
df["Source name"] = df["Source Content"]

# Rename sources to generic names
df["Source Content"] = [f"Source {i+1}" for i in range(len(df))]

# Rename references to generic names while storing original names
reference_columns = df.columns[1:-1]  # Exclude first and last column (source & hover info)
new_reference_names = {col: f"Ref {i+1}" for i, col in enumerate(reference_columns)}
df.rename(columns=new_reference_names, inplace=True)

# Convert to long format for plotting
df_long = df.melt(id_vars=["Source Content", "Source name"], var_name="Reference", value_name="Similarity")

# Map back to original reference names for hover
df_long["Reference name"] = df_long["Reference"].map({v: k for k, v in new_reference_names.items()})

# Use original reference names in legend
df_long["Legend Reference"] = df_long["Reference"] + ": " + df_long["Reference name"]

fig = px.scatter_3d(
        df_long,  
        x='Source Content',
        y='Reference', 
        z='Similarity', 
        color="Legend Reference", 
        # symbol='Reference Index',
        labels={ 'Source Content': 'Source', 'Reference': 'Reference', "Legend Reference": "Reference", 'Similarity': 'Similarity Score',},
        title="3D Similarity Score Plot",
        hover_data={"Source name": True, "Reference name": True, "Similarity": True, "Source Content": False, "Reference": False}  # Hide generic names in hover
    )

# Update Labels for 3D scatter
fig.update_layout(
    scene=dict(
        xaxis=dict(title='Source Content', tickangle=-30, tickfont=dict(size=12)),  # Hide Source Labels
        yaxis=dict(title="Reference", tickangle=-30, tickfont=dict(size=12)),  #use "showticklabels=False" to hide label
        zaxis=dict(title="Similarity Score", range=[-1, 1]),
    ),
    margin=dict(l=50, r=50, b=100, t=50),
    autosize=True
)

# Show plot
fig.show()