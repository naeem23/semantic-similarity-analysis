import pandas as pd
import plotly.graph_objects as go
import plotly.express as px
from plotly.subplots import make_subplots
import os

def readCsv(file_path):
    """Reads CSV file and returns a pandas DataFrame"""
    if not os.path.exists(file_path):
        raise FileNotFoundError(f"File {file_path} does not exist.")
    try:
        df = pd.read_csv(file_path)
    except Exception as e:
        raise Exception(f"Error reading the CSV file: {e}")
    return df

def generateBarChart(df, reference):
    """Generates a bar chart for a specific reference"""
    # Filter rows for the given reference
    ref_data = df[df['Reference'] == reference]

    if ref_data.empty:
        raise ValueError(f"No data found for reference: {reference}")
    
    # Ensure the required columns exist
    required_columns = {'Source', 'ScoreAda', 'ScoreSmall', 'ScoreLarge'}
    missing_columns = required_columns - set(df.columns)
    if missing_columns:
        raise KeyError(f"Missing columns in DataFrame: {missing_columns}")
    
    sources = ref_data['Source'].tolist()
    scores_ada = ref_data['ScoreAda'].tolist()
    scores_small = ref_data['ScoreSmall'].tolist()
    scores_large = ref_data['ScoreLarge'].tolist()

    # Create bar chart
    fig = go.Figure()
    fig.add_trace(go.Bar(
        x=sources, 
        y=scores_ada, 
        name='text-embedding-ada-002', 
        marker_color='#636efa',
        hovertext=[
            f"Source: {src}<br>Model: text-embedding-ada-002<br>Score: {score:.2f}" 
            for src, score in zip(sources, scores_ada)
        ],
        hoverinfo="text"
    ))
    fig.add_trace(go.Bar(
        x=sources, 
        y=scores_small, 
        name='text-embedding-3-small', 
        marker_color='#ef553b',
        hovertext=[
            f"Source: {src}<br>Model: text-embedding-3-small<br>Score: {score:.2f}" 
            for src, score in zip(sources, scores_ada)
        ],
        hoverinfo="text"
    ))
    fig.add_trace(go.Bar(
        x=sources, 
        y=scores_large, 
        name='text-embedding-3-large', 
        marker_color='#00cc96',
        hovertext=[
            f"Source: {src}<br>Model: text-embedding-3-large<br>Score: {score:.2f}" 
            for src, score in zip(sources, scores_ada)
        ],
        hoverinfo="text"
    ))

    # Update layout
    fig.update_layout(
        title=f"Cosine similarity for {reference}",
        xaxis=dict(
            title="Sources",
            showline=True,
            showgrid=False
        ),
        yaxis=dict(
            title="Similarity Score",
            showline=True,
            showgrid=True,
            gridcolor="#ced4da"
        ),
        # yaxis=dict(range=[-1, 1]),
        barmode='group',
        template="plotly_white"
    )
    return fig


def plotAllCharts(df):
    """Generates subplots with one chart per reference"""
    unique_references = df['Reference'].unique()
    
    # Create subplots with number of rows equal to the number of unique references
    num_references = len(unique_references)
    fig = make_subplots(
        rows=num_references, 
        cols=1, 
        subplot_titles=[f"Cosine similarity for \"{ref}\"" for ref in unique_references],
    )
    
    row = 1
    for reference in unique_references:
        # Generate bar chart for each reference and add it to the subplot
        bar_chart = generateBarChart(df, reference)
        for trace in bar_chart.data:
            trace.showlegend = row == 1  # Show legend only for the first subplot
            fig.add_trace(trace, row=row, col=1)

        # Set x-axis and y-axis titles for each subplot
        fig.update_xaxes(title_text="Sources", row=row, col=1, tickmode='array')
        fig.update_yaxes(title_text="Similarity Score", row=row, col=1, gridcolor="#ced4da")  # Set grid color
        
        row += 1

    # Update the layout of the entire figure
    fig.update_layout(
        height=400 * num_references,  # Adjust height based on the number of charts
        showlegend=True,
        title_text="Cosine Similarity for Different References",
        template="plotly_white",
        plot_bgcolor="white"  # Ensure the entire background is white
    )
    fig.show()


def main(csv_file_path):
    """Main function to read CSV and plot all charts"""
    try:
        df = readCsv(csv_file_path)
    except Exception as e:
        print(f"Error: {e}")
        return
    
    try:
        plotAllCharts(df)
    except Exception as e:
        print(f"Error during plotting: {e}")


if __name__ == "__main__":
    # Get file path input from the user
    csv_file_path = input("Please enter the path to the CSV file: ")
    main(csv_file_path)
