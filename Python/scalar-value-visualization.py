import pandas as pd
import numpy as np
import plotly.subplots as sp
import plotly.graph_objects as go
import os

def read_csv(file_path):
    """Reads a CSV file and returns a DataFrame."""
    try:
        df = pd.read_csv(file_path)
        if df.empty:
            raise ValueError("CSV file is empty.")
        return df
    except Exception as e:
        print(f"Error reading file {file_path}: {e}")
        return None


def cosine_similarity(vec1, vec2):
    """Computes cosine similarity between two vectors."""
    try:
        vec1, vec2 = np.array(vec1), np.array(vec2)
        dot_product = np.dot(vec1, vec2)
        norm_product = np.linalg.norm(vec1) * np.linalg.norm(vec2)
        if norm_product == 0:
            return 0
        return dot_product / norm_product
    except Exception as e:
        print(f"Error computing cosine similarity: {e}")
        return None


def plot_scatter_graph(source, reference, source_name, reference_name, subplot_row, subplot_col, fig, index):
    """Creates scatter plots for a source-reference pair."""
    x_values = np.linspace(0, 536, len(source))
    y_values = np.interp(source, (min(source), max(source)), (-1, 1))
    
    trace = go.Scatter(
        x=x_values, 
        y=y_values, 
        mode='markers', 
        name=f'{source_name}',
        legendgroup=f'group{index}',  # Unique legend group per subplot
        showlegend=True  # Ensure legend appears for each subplot
    )
    fig.add_trace(trace, row=subplot_row, col=subplot_col)
    
    x_values_ref = np.linspace(0, 536, len(reference))
    y_values_ref = np.interp(reference, (min(reference), max(reference)), (-1, 1))
    
    trace_ref = go.Scatter(
        x=x_values_ref, 
        y=y_values_ref, 
        mode='markers', 
        name=f'{reference_name}',
        legendgroup=f'group{index}',  # Unique legend group per subplot
        showlegend=True  # Ensure legend appears for each subplot
    )
    fig.add_trace(trace_ref, row=subplot_row, col=subplot_col)


def main(source_file, reference_file):
    """Main function to read CSVs, compute similarity, and plot graphs."""
    source_df = read_csv(source_file)
    reference_df = read_csv(reference_file)
    
    if source_df is None or reference_df is None:
        print("Error reading CSV files. Exiting.")
        return
    
    source_names = source_df.iloc[:, 0].values  # First column as source names
    sources = source_df.iloc[:, 1:].values  # Exclude first column (source names)

    reference_names = reference_df.iloc[:, 0].values  # First column as reference names
    references = reference_df.iloc[:, 1:].values  # Exclude first column (reference names)
    
    num_sources = len(sources)
    num_references = len(references)
    total_plots = num_sources * num_references
    
    fig = sp.make_subplots(
        rows=total_plots, 
        cols=1, 
        subplot_titles=[f"{source_names[i]} vs {reference_names[i]}" for i in range(num_sources) for j in range(num_references)]
    )

    plot_index = 0  # Track subplot index
    
    for i, source in enumerate(sources):
        for j, reference in enumerate(references):
            row = plot_index + 1  # Each plot in its own row
            col = 1  # Always one column
            similarity = cosine_similarity(source, reference)
            print(f"Cosine Similarity (Source {i+1}, Reference {j+1}): {similarity:.4f}")
            plot_scatter_graph(source, reference, source_names[i], reference_names[j], row, col, fig, plot_index)
            plot_index += 1  # Move to the next row

            # Set x-axis and y-axis titles for each subplot
            fig.update_xaxes(title_text="Scalar value", row=row, col=1)
            fig.update_yaxes(title_text="Similarity Score", row=row, col=1)
    
    fig.update_layout(height=500 * total_plots, width=900, title_text="Source-Reference Scatter Plots")
    fig.show()


if __name__ == "__main__":
    # source_csv_path = "source_scalar_values.csv"
    # reference_csv_path = "reference_scalar_values.csv"

    source_csv_path = input(r"Please enter the file path for the source CSV: ")
    reference_csv_path = input(r"Please enter the file path for the reference CSV: ")
    
    if not os.path.exists(source_csv_path) or not os.path.exists(reference_csv_path):
        print("One or both CSV files are missing. Please provide valid files.")
    else:
        main(source_csv_path, reference_csv_path)


# G:\FUAS\SE\semantic-similarity-analysis\SemanticSimilarity\Output\source_scalar_values.csv
# G:\FUAS\SE\semantic-similarity-analysis\SemanticSimilarity\Output\reference_scalar_values.csv