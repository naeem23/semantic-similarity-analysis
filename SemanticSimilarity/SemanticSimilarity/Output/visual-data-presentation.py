import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
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
    Creates a grouped scatter plot using Plotly and saves it as an HTML file.
    """
    try:
        # Shorten 'Source' and 'Reference' to make labels concise
        df['Short_Source'] = df['Source'].apply(extract_word)
        df['Short_Reference'] = df['Reference'].apply(extract_word)

        # Combine 'Source' and 'Reference' into a single label for the X-axis
        df['Source_Reference'] = df['Short_Source'] + '-' + df['Short_Reference']

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
            range_y=[-1, 1]  # Set Y-axis range from -1 to 1
        )

        # Update layout for better readability
        fig.update_layout(
            xaxis_title="Source-Reference Pairs",
            yaxis_title="Similarity Score",
            showlegend=True,  # Show legend
            template="plotly_white"  # Use a clean template
        )

        # Show the plot directly
        fig.show()

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