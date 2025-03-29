import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
from plotly.subplots import make_subplots
import re

def read_csv(file_path):
    """
    Reads the CSV file and returns a DataFrame.
    Author: Naeem
    """
    try:
        df = pd.read_csv(file_path)
        return df
    except FileNotFoundError:
        print(f"Error: The file '{file_path}' was not found.")
    except Exception as e:
        print(f"An error occurred while reading the file: {e}")
    return None


def extract_word(text):
    """
    Get first word from source and refernce 
    """
    stopwords = {"a", "an", "the", "in", "on", "at", "to", "for", "with", "by", "of", "and", 
    "or", "but", "nor", "so", "yet", "is", "are", "was", "were", "be", "been", "being", "has", 
    "have", "had", "do", "does", "did", "this", "that", "these", "those", "some", "any", "each", 
    "every", "either", "neither"}
    
    words = re.findall(r"\w+", text)
    if not words:
        return ""
    elif words.count == 1:
        return words
    else:
        return words[0] if words[0].lower() not in stopwords else " ".join(words[:2])


def create_grouped_scatter_plot(df):
    """
    Creates a grouped scatter plot using Plotly.
    Returns the figure object
    """
    try:
        # Shorten 'Source' and 'Reference' to make labels concise
        df['Short_Source'] = df['Source'].apply(extract_word)
        df['Short_Reference'] = df['Reference'].apply(extract_word)

        # Combine 'Source' and 'Reference' into a single label for the X-axis
        df['Source_Reference'] = df['Short_Source'] + ' vs ' + df['Short_Reference']

        # Melt the DataFrame to long format for Plotly Express
        df_melted = df.melt(
            id_vars=['Source_Reference', 'Source', 'Reference'],  # Keep these columns
            value_vars=['Score_Ada', 'Score_Small', 'Score_Large'],  # Columns to melt
            var_name='Model',  # New column for model names
            value_name='Score'  # New column for similarity scores
        )
        
        # Create the scatter plot using Plotly Express
        fig = px.scatter(
            df_melted,
            x='Source_Reference',  # X-axis: Shortened Source-Reference pairs
            y='Score',             # Y-axis: Similarity scores
            color='Model',         # Color by model
            hover_data={'Source': True, 'Reference': True, 'Model': True, 'Score': ':.2f'},  # Hover data
            title="Grouped Scatter Plot of Similarity Scores",
            labels={'Source_Reference': 'Source-Reference Pairs', 'Score': 'Similarity Score'},
            # range_y=[-1, 1]  # Set Y-axis range from -1 to 1
        )

        # Update layout for better readability
        fig.update_layout(
            xaxis_title="Source-Reference Pairs",
            yaxis_title="Similarity Score",
            showlegend=True,  # Show legend
            template="plotly_white",  # Use a clean template
            # height=400 # Set height for the subplot
        )

        # Show the plot directly
        # fig.show()

        return fig

    except KeyError as e:
        print(f"Error: The required column '{e}' is missing in the CSV file.")
        return None
    except Exception as e:
        print(f"An error occurred while creating the plot: {e}")
        return None


def create_grouped_bar_chart(df):
    """
    Creates a grouped bar chart similar to the provided image.
    Returns the figure object.
    """
    try:
        # Shorten 'Source' and 'Reference' to make labels concise
        df['Short_Source'] = df['Source'].apply(extract_word)
        df['Short_Reference'] = df['Reference'].apply(extract_word)

        # Combine 'Source' and 'Reference' into a single label for the X-axis
        df['Source_Reference'] = df['Short_Source'] + ' vs ' + df['Short_Reference']

        # Create the figure
        fig = go.Figure()

        # Add bars for each model
        model_colors = {'Score_Ada': '#636efa', 'Score_Small': '#ef553b', 'Score_Large': '#00cc96'}
        
        for model, color in model_colors.items():
            fig.add_trace(go.Bar(
                x=df['Source_Reference'],
                y=df[model],
                name=model,
                marker_color=color,
                hovertext=df.apply(lambda row: f"Source: {row['Source']}<br>Reference: {row['Reference']}<br>Model: {model.replace('Score_', '')}<br>Score: {row[model]:.2f}", axis=1),
                hoverinfo="text"
            ))

        # Update layout
        fig.update_layout(
            barmode='group',  # Grouped bars
            title="Grouped Bar Chart of Similarity Scores",
            xaxis_title="Source-Reference Pairs",
            yaxis_title="Similarity Score",
            # yaxis_range=[-1, 1],  # Set Y-axis range from -1 to 1
            showlegend=True,
            template="plotly_white",
            # height=400  # Set height for the subplot
        )

        return fig

    except KeyError as e:
        print(f"Error: The required column '{e}' is missing in the CSV file.")
        return None
    except Exception as e:
        print(f"An error occurred while creating the plot: {e}")
        return None


def create_combined_visualization(df):
    """
    Creates a combined visualization with scatter plot on top and bar chart below.
    """
    # Create both figures
    scatter_fig = create_grouped_scatter_plot(df)
    bar_fig = create_grouped_bar_chart(df)
    
    if scatter_fig is None or bar_fig is None:
        return None

    # Create subplots with 2 rows and 1 column
    fig = make_subplots(
        rows=2, 
        cols=1,
        subplot_titles=("Grouped Scatter Plot of Similarity Scores", 
                       "Grouped Bar Chart of Similarity Scores"),
        vertical_spacing=0.4, # Increased gap between charts
    )

    # Add scatter plot traces to first row
    for trace in scatter_fig.data:
        fig.add_trace(trace, row=1, col=1)

    # # Add bar chart traces to second row
    # for trace in bar_fig.data:
    #     fig.add_trace(trace, row=2, col=1)

    # Add bar chart traces to second row - using the same colors as scatter plot
    model_colors = {'Score_Ada': '#636efa', 'Score_Small': '#ef553b', 'Score_Large': '#00cc96'}
    for model, color in model_colors.items():
        fig.add_trace(go.Bar(
            x=df['Source_Reference'],
            y=df[model],
            name=model,
            marker_color=color,
            showlegend=False,  # Don't show legend for bar chart traces
            hovertext=df.apply(lambda row: f"Source: {row['Source']}<br>Reference: {row['Reference']}<br>Model: {model.replace('Score_', '')}<br>Score: {row[model]:.2f}", axis=1),
            hoverinfo="text"
        ), row=2, col=1)


    # Update layout for the combined figure
    fig.update_layout(
        height=800,  # Total height of the figure
        showlegend=True,
        template="plotly_white",
        legend_title_text='Models',
        xaxis_title="Source-Reference Pairs",
        yaxis_title="Similarity Score",
        xaxis2_title="Source-Reference Pairs",
        yaxis2_title="Similarity Score",
        # yaxis_range=[-1, 1],  # For first subplot
        # yaxis2_range=[-1, 1]  # For second subplot
    )

    # Update x-axis properties for both subplots
    fig.update_xaxes(tickangle=-45, row=1, col=1)
    fig.update_xaxes(tickangle=-45, row=2, col=1)

    return fig


def main():
    # File path to the CSV
    file_path = input(r"Please enter the file path for the similarity results CSV: \n")

    # Read the CSV file
    df = read_csv(file_path)
    if df is not None:
        # Create the grouped scatter plot
        # create_grouped_scatter_plot(df)
        combined_fig = create_combined_visualization(df)
        if combined_fig is not None:
            combined_fig.show()


if __name__ == "__main__":
    main()