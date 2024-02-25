all : Out.txt  Out.rMaxConvergence.gnuplot.png Out.drConvergence.gnuplot.png Out.waveVectors.gnuplot.png

vpath %.dll Libs
LIBS = matrix.dll vector.dll
LIBSREF = -reference:matrix.dll -reference:vector.dll

Out.rMaxConvergence.gnuplot.png : Out.rMaxConvergenceData.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "r_{max}" ;\
			set ylabel "GS-energy (Hartree)" ;\
			set grid ;\
			set title "Convergence of GS-energy for varying r_{max}" ;\
			plot "$<" with lines lw 3 linecolor rgb "#FC695E" notitle ;\
		' | gnuplot

Out.drConvergence.gnuplot.png : Out.drConvergenceData.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "dr" ;\
			set ylabel "GS-energy (Hartree)" ;\
			set grid ;\
			set title "Convergence of GS-energy for varying dr" ;\
			plot "$<" with lines lw 3 linecolor rgb "#FC695E" notitle ;\
		' | gnuplot
	
Out.waveVectors.gnuplot.png : 
	echo '\
			set terminal png background "white" size "700, 1000" ;\
			set out "$@" ;\
			set grid ;\
			set key top center box height 1 ;\
			;\
			set multiplot layout 2, 1 spacing 0.08, 0.08 ;\
			;\
			set size 1, 0.52 ;\
			set format x " " ;\
			set yrange [0: 1] ;\
			set ytics 0.1, 0.2, 0.9 ;\
			set ylabel "Prob. amp." ;\
			set title "Wave vector corresponding to radial quantum numbers k" ;\
			plot "Out.waveVectorsData.txt" using 1:2 with lines lw 3 linecolor rgb "#F05D5A" title "Numerical (k = 1)" ;\
			;\
			unset title ;\
			set size 1, 0.52 ;\
			unset format x ;\
			set yrange [-0.6: 0.4] ;\
			set ytics -0.5, 0.2, 0.3 ;\
			set xlabel "r (in units of Bohr radius)" ;\
			set ylabel "Prob. amp." ;\
			plot "Out.waveVectorsData.txt" using 1:3 with lines lw 3 linecolor rgb "#F05D5A" title "Numerical (k = 2)" ;\
			;\
			unset multiplot ;\
		' | gnuplot

Out.rMaxConvergenceData.txt : convergence.exe
	> $@
	for rMax in $$(seq 3 0.35 10); do \
		mono $< -rMax:$$rMax -dr:0.3 -test:rMax >> $@; \
	done

Out.drConvergenceData.txt : convergence.exe
	> $@
	for dr in $$(seq 0.05 0.05 0.5); do \
		mono $< -rMax:14 -dr:$$dr -test:dr >> $@; \
	done
	
convergence.exe : convergence.cs evd.dll $(LIBS)
	mcs -target:exe -reference:evd.dll $(LIBSREF) -out:$@ $<

Out.txt : main.exe
	mono $< -rMax:14 -dr:0.05 -outFile:Out.waveVectorsData.txt > $@

main.exe : main.cs evd.dll $(LIBS)
	mcs -target:exe -reference:evd.dll $(LIBSREF) -out:$@ $<

evd.dll : evd.cs $(LIBS)
	mcs -target:library $(LIBSREF) -out:$@ $<

clean :
	$(RM) *.exe [Oo]ut.* evd.dll