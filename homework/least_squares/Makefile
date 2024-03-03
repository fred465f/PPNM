all: Out.fitActivity.gnuplot.svg

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image files.
Out.fitActivity.gnuplot.svg : Out.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "t (days)" font ", 14" ;\
			set ylabel "Activity (relative units)" font ", 14" ;\
			set title "Activity of ThX" font ", 14" ;\
			set grid ;\
			set key box height 1 width -6 font ", 14" at screen 0.90, 0.82 ;\
			set xrange [0: 16] ;\
			a = 141.338164625218; lambda = 0.169887082001008 ;\
			plot "inputActivityData.txt" title "Data" lt 7 lc 0 with errorbars, \
			"Out.fitActivityData.txt" using 1:2 with lines lc 7 lw 2 title sprintf("Fit: %.f * e^{-%.2f}", a, lambda), \
			"Out.fitActivityData.txt" using 1:3 with lines dashtype 2 lc 6 lw 1 title "Fit: param. -= error", \
			"Out.fitActivityData.txt" using 1:4 with lines dashtype 2 lc 6 lw 1 title "Fit: param. += error" ;\
		' | gnuplot

# Main output to commandline
Out.txt : main.exe
	mono $< -inFile:inputActivityData.txt -outFile:Out.fitActivityData.txt > $@

# Executables.
main.exe : main.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll