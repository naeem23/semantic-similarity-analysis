import pandas as pd
import plotly.express as px
import plotly.graph_objects as go


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


def create_grouped_scatter_plot(df, output_file):
    """
    Creates a grouped scatter plot using Plotly and saves it as an HTML file.
    """
    try:
        # Combine 'Source' and 'Reference' into a single label for the X-axis
        df['Source_Reference'] = df['Source'] + '-' + df['Reference']

        # Create a scatter plot for each model
        fig = go.Figure()

        # Add traces for each model
        models = ['Score_Ada', 'Score_Small', 'Score_Large']
        colors = ['blue', 'green', 'red']  # Different colors for each model
        for model, color in zip(models, colors):
            fig.add_trace(
                go.Scatter(
                    x=df['Source_Reference'],  # X-axis: Source-Reference pairs
                    y=df[model],               # Y-axis: Similarity scores
                    mode='markers',            # Scatter plot
                    name=model,                # Legend label
                    marker=dict(color=color),  # Color for the model
                    text=df.apply(lambda row: f"Source: {row['Source']}<br>Reference: {row['Reference']}<br>Model: {model}<br>Score: {row[model]:.2f}", axis=1),  # Hover text
                    hoverinfo='text'           # Show custom hover text
                )
            )

        # Update layout for better readability
        fig.update_layout(
            title="Grouped Scatter Plot of Similarity Scores",
            xaxis_title="Source-Reference Pairs",
            yaxis_title="Similarity Score",
            yaxis_range=[-1, 1],  # Set Y-axis range from -1 to 1
            showlegend=True,        # Show legend
            template="plotly_white" # Use a clean template
        )

        # Save the plot as an HTML file
        fig.write_html(output_file)
        print(f"Plot saved successfully as '{output_file}'.")

    except KeyError as e:
        print(f"Error: The required column '{e}' is missing in the CSV file.")
    except Exception as e:
        print(f"An error occurred while creating the plot: {e}")

def main():
    # File path to the CSV
    file_path = "similarity_results.csv"

    # Read the CSV file
    df = read_csv(file_path)
    if df is not None:
        # Create the grouped scatter plot
        create_grouped_scatter_plot(df)

if __name__ == "__main__":
    main()